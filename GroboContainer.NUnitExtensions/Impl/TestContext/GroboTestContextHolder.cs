using System;
using System.Collections.Concurrent;
using System.Reflection;

using JetBrains.Annotations;

using NUnit.Framework.Interfaces;

using NUnitTestContext = NUnit.Framework.TestContext;

namespace GroboContainer.NUnitExtensions.Impl.TestContext
{
    public static class GroboTestContextHolder
    {
        [NotNull]
        public static GroboTestContext GetCurrentContext()
        {
            var testName = NUnitTestContext.CurrentContext.Test.FullName;
            var testId = NUnitTestContext.CurrentContext.Test.ID;
            if (!methodContexts.TryGetValue(testId, out var currentMethodContext))
                throw new InvalidOperationException($"TestContext for test with name: {testName}, id: {testId} is not set");

            var suiteName = GetTestMethodInfo().GetSuiteName();
            if (!suiteContexts.TryGetValue(suiteName, out var currentSuiteContext))
                throw new InvalidOperationException($"SuiteContext for test with name: {testName}, id: {testId} is not set");

            return new GroboTestContext(testName, currentSuiteContext, currentMethodContext);
        }

        public static void SetCurrentContext([NotNull] GroboTestSuiteContextData suiteContext, [NotNull] GroboTestMethodContextData methodContext)
        {
            var testName = NUnitTestContext.CurrentContext.Test.FullName;
            var testId = NUnitTestContext.CurrentContext.Test.ID;
            if (!methodContexts.TryAdd(testId, methodContext))
                throw new InvalidOperationException($"MethodContext for test with id: {testId}, name: {testName} already exists");

            var suiteName = GetTestMethodInfo().GetSuiteName();
            suiteContexts.TryAdd(suiteName, suiteContext);
        }

        [NotNull]
        public static GroboTestMethodContextData GetCurrentMethodContext()
        {
            var testName = NUnitTestContext.CurrentContext.Test.FullName;
            var testId = NUnitTestContext.CurrentContext.Test.ID;
            if (!methodContexts.TryGetValue(testId, out var currentMethodContext))
                throw new InvalidOperationException($"Unable to get TestContext for test with id: {testId}, name: {testName}");
            return currentMethodContext;
        }

        public static void ResetCurrentMethodContext()
        {
            var testName = NUnitTestContext.CurrentContext.Test.FullName;
            var testId = NUnitTestContext.CurrentContext.Test.ID;
            if (!methodContexts.TryRemove(testId, out _))
                throw new InvalidOperationException($"Unable to remove TestContext for test with id: {testId}, name: {testName}");
        }

        private static MethodInfo GetTestMethodInfo()
        {
            var testAdapter = NUnitTestContext.CurrentContext.Test;
            var test = (ITest)testField.GetValue(testAdapter);
            return test.Method.MethodInfo;
        }

        private static readonly FieldInfo testField = typeof(NUnitTestContext.TestAdapter).GetField("_test", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly ConcurrentDictionary<string, GroboTestSuiteContextData> suiteContexts = new ConcurrentDictionary<string, GroboTestSuiteContextData>();
        private static readonly ConcurrentDictionary<string, GroboTestMethodContextData> methodContexts = new ConcurrentDictionary<string, GroboTestMethodContextData>();
    }
}