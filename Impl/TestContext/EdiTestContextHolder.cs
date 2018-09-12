using System.Collections.Concurrent;
using System.Reflection;

using JetBrains.Annotations;

using NUnit.Framework.Interfaces;

using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public static class EdiTestContextHolder
    {
        [NotNull]
        public static EdiTestContext GetCurrentContext()
        {
            var testName = global::NUnit.Framework.TestContext.CurrentContext.Test.FullName;
            var testId = global::NUnit.Framework.TestContext.CurrentContext.Test.ID;
            var suiteName = GetTestMethodInfo().GetSuiteName();
            if (!methodContexts.TryGetValue(testId, out var currentMethodContext))
                throw new InvalidProgramStateException("TestContext for test with name: {testName}, id: {testId} is not set");
            if (!suiteContexts.TryGetValue(suiteName, out var currentSuiteContext))
                throw new InvalidProgramStateException("SuiteContext for test with name: {testName}, id: {testId} is not set");

            return new EdiTestContext(testName, currentSuiteContext, currentMethodContext);
        }

        public static void SetCurrentContext([NotNull] EdiTestSuiteContextData suiteContext, [NotNull] EdiTestMethodContextData methodContext)
        {
            var testName = global::NUnit.Framework.TestContext.CurrentContext.Test.FullName;
            var testId = global::NUnit.Framework.TestContext.CurrentContext.Test.ID;
            if (!methodContexts.TryAdd(testId, methodContext))
                throw new InvalidProgramStateException($"MethodContext for test with id: {testId}, name: {testName} already exists");

            var suiteName = GetTestMethodInfo().GetSuiteName();
            suiteContexts.TryAdd(suiteName, suiteContext);
        }

        [NotNull]
        public static EdiTestMethodContextData ResetCurrentTestContext()
        {
            var testName = global::NUnit.Framework.TestContext.CurrentContext.Test.FullName;
            var testId = global::NUnit.Framework.TestContext.CurrentContext.Test.ID;
            if (!methodContexts.TryRemove(testId, out var currentMethodContext))
                throw new InvalidProgramStateException($"Unable to remove TestContext for test with id: {testId}, name: {testName}");
            return currentMethodContext;
        }

        private static MethodInfo GetTestMethodInfo()
        {
            var testAdapter = global::NUnit.Framework.TestContext.CurrentContext.Test;
            var test = (ITest)testField.GetValue(testAdapter);
            return test.Method.MethodInfo;
        }

        private static readonly FieldInfo testField = typeof(global::NUnit.Framework.TestContext.TestAdapter).GetField("_test", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly ConcurrentDictionary<string, EdiTestSuiteContextData> suiteContexts = new ConcurrentDictionary<string, EdiTestSuiteContextData>();
        private static readonly ConcurrentDictionary<string, EdiTestMethodContextData> methodContexts = new ConcurrentDictionary<string, EdiTestMethodContextData>();
    }
}