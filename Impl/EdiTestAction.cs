using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;
using SKBKontur.Catalogue.NUnit.Extensions.TestEnvironments.ExceptionUtils;
using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl
{
    public static class EdiTestAction
    {
        public static void BeforeTest([NotNull] TestDetails testDetails)
        {
            EnsureAppDomainIntialization();

            var test = testDetails.Method;
            test.EnsureNunitAttributesAbscence();
            var fixtureType = test.GetFixtureType();
            var testFixture = testDetails.Fixture;
            if(fixtureType != testFixture.GetType())
                throw new InvalidProgramStateException(string.Format("TestFixtureType mismatch for: {0}", test.GetMethodName()));

            var suiteName = test.GetSuiteName();
            var suiteDescriptor = suiteDescriptors.GetOrAdd(suiteName, x => new SuiteDescriptor(fixtureType.Assembly));
            var suiteContext = suiteDescriptor.SuiteContext;
            foreach(var suiteWrapper in test.GetSuiteWrappers())
            {
                var setUpedWrappersForSuite = setUpedSuiteWrappers.GetOrAdd(suiteName, x => new List<EdiTestSuiteWrapperAttribute>());
                if(!setUpedWrappersForSuite.Contains(suiteWrapper))
                {
                    suiteWrapper.SetUp(suiteName, suiteDescriptor.TestAssembly, suiteContext);
                    setUpedWrappersForSuite.Add(suiteWrapper);
                }
            }

            if(setUpedFixtures.Add(fixtureType))
            {
                InjectFixtureFields(suiteContext, testFixture);
                var fixtureSetUpMethod = test.FindFixtureSetUpMethod();
                if(fixtureSetUpMethod != null)
                {
                    if(suiteName != fixtureType.FullName)
                        throw new InvalidProgramStateException(string.Format("EdiTestFixtureSetUp method is only allowed inside EdiTestFixure suite. Test: {0}", test.GetMethodName()));
                    InvokeWrapperMethod(fixtureSetUpMethod, testFixture, suiteContext.GetContainer());
                }
            }

            var testName = testDetails.FullName;
            var methodContext = new EdiTestMethodContextData();
            foreach(var methodWrapper in test.GetMethodWrappers())
                methodWrapper.SetUp(testName, suiteContext, methodContext);

            EdiTestContextHolder.SetCurrentTestContext(testName, suiteContext, methodContext);

            InvokeWrapperMethod(test.FindSetUpMethod(), testFixture);
        }

        public static void AfterTest([NotNull] TestDetails testDetails)
        {
            var test = testDetails.Method;
            var suiteName = test.GetSuiteName();
            SuiteDescriptor suiteDescriptor;
            if(!suiteDescriptors.TryGetValue(suiteName, out suiteDescriptor))
                throw new InvalidProgramStateException(string.Format("Suite context is not set for: {0}", suiteName));

            InvokeWrapperMethod(test.FindTearDownMethod(), testDetails.Fixture);

            // todo [edi-test]: duplicate call fails when [TestSuite] is defined mltiple times for the given test
            var methodContext = EdiTestContextHolder.ResetCurrentTestContext();

            var testName = testDetails.FullName;
            foreach(var methodWrapper in Enumerable.Reverse(test.GetMethodWrappers()))
                methodWrapper.TearDown(testName, suiteDescriptor.SuiteContext, methodContext);
        }

        private static void InvokeWrapperMethod([CanBeNull] MethodInfo wrapperMethod, [NotNull] object testFixture, params object[] @params)
        {
            if(wrapperMethod == null)
                return;
            try
            {
                wrapperMethod.Invoke(testFixture, @params);
            }
            catch(TargetInvocationException exception)
            {
                exception.RethrowInnerException();
            }
        }

        private static void InjectFixtureFields([NotNull] EdiTestSuiteContextData suiteContext, [NotNull] object testFixture)
        {
            foreach(var fieldInfo in testFixture.GetType().GetFieldsForInjection())
                fieldInfo.SetValue(testFixture, suiteContext.GetContainer().Get(fieldInfo.FieldType));
        }

        private static void EnsureAppDomainIntialization()
        {
            if(appDomainIsIntialized)
                return;
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => TearDownSuiteWrappers();
            appDomainIsIntialized = true;
        }

        private static void TearDownSuiteWrappers()
        {
            foreach(var kvp in setUpedSuiteWrappers)
            {
                var suiteName = kvp.Key;
                var setUpedWrappersForSuite = kvp.Value;
                foreach(var suiteWrapper in Enumerable.Reverse(setUpedWrappersForSuite))
                {
                    var suiteDescriptor = suiteDescriptors[suiteName];
                    suiteWrapper.TearDown(suiteName, suiteDescriptor.TestAssembly, suiteDescriptor.SuiteContext);
                }
            }
        }

        private static bool appDomainIsIntialized;
        private static readonly HashSet<Type> setUpedFixtures = new HashSet<Type>();
        private static readonly ConcurrentDictionary<string, SuiteDescriptor> suiteDescriptors = new ConcurrentDictionary<string, SuiteDescriptor>();
        private static readonly ConcurrentDictionary<string, List<EdiTestSuiteWrapperAttribute>> setUpedSuiteWrappers = new ConcurrentDictionary<string, List<EdiTestSuiteWrapperAttribute>>();

        private class SuiteDescriptor
        {
            public SuiteDescriptor([NotNull] Assembly testAssembly)
            {
                TestAssembly = testAssembly;
                SuiteContext = new EdiTestSuiteContextData();
            }

            [NotNull]
            public Assembly TestAssembly { get; private set; }

            [NotNull]
            public EdiTestSuiteContextData SuiteContext { get; private set; }
        }
    }
}