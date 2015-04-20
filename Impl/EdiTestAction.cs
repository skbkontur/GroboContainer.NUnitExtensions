using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GroboContainer.Core;
using GroboContainer.Impl;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;
using SKBKontur.Catalogue.Objects;
using SKBKontur.Catalogue.ServiceLib;
using SKBKontur.Catalogue.ServiceLib.Logging;

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
                if(suiteDescriptor.SetUpedSuiteWrappers.Contains(suiteWrapper))
                    continue;
                suiteWrapper.SetUp(suiteName, suiteDescriptor.TestAssembly, suiteContext);
                suiteDescriptor.SetUpedSuiteWrappers.Add(suiteWrapper);
            }

            if(setUpedFixtures.Add(fixtureType))
            {
                var fixtureSetUpMethod = test.FindFixtureSetUpMethod();
                if(fixtureSetUpMethod != null)
                {
                    if(suiteName != fixtureType.FullName)
                        throw new InvalidProgramStateException(string.Format("EdiTestFixtureSetUp method is only allowed inside EdiTestFixture suite. Test: {0}", test.GetMethodName()));
                    InvokeWrapperMethod(fixtureSetUpMethod, testFixture, suiteContext);
                }
                InjectFixtureFields(suiteContext, testFixture);
            }

            var testName = testDetails.FullName;
            var methodContext = new EdiTestMethodContextData(suiteDescriptor.LazyContainer);
            foreach(var methodWrapper in test.GetMethodWrappers())
                methodWrapper.SetUp(testName, suiteContext, methodContext);

            EdiTestContextHolder.SetCurrentContext(testName, suiteContext, methodContext);

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

            var methodContext = EdiTestContextHolder.ResetCurrentTestContext();

            var testName = testDetails.FullName;
            foreach(var methodWrapper in Enumerable.Reverse(test.GetMethodWrappers()))
                methodWrapper.TearDown(testName, suiteDescriptor.SuiteContext, methodContext);

            AggregateException error;
            if(!methodContext.TryDestroy(out error))
                throw error;
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
                fieldInfo.SetValue(testFixture, suiteContext.Container.Get(fieldInfo.FieldType));
        }

        private static void EnsureAppDomainIntialization()
        {
            if(appDomainIsIntialized)
                return;
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => OnAppDomainUnload();
            appDomainIsIntialized = true;
        }

        private static void OnAppDomainUnload()
        {
            var suiteDescriptorsInOrderOfDestruction = suiteDescriptors.OrderByDescending(x => x.Value.Order).ToList();
            Log.For("EdiTestMachinery").InfoFormat("Suites to tear down: {0}", string.Join(", ", suiteDescriptorsInOrderOfDestruction.Select(x => x.Key)));
            foreach(var kvp in suiteDescriptorsInOrderOfDestruction)
            {
                var suiteName = kvp.Key;
                var suiteDescriptor = kvp.Value;
                foreach(var suiteWrapper in Enumerable.Reverse(suiteDescriptor.SetUpedSuiteWrappers))
                    suiteWrapper.TearDown(suiteName, suiteDescriptor.TestAssembly, suiteDescriptor.SuiteContext);
                suiteDescriptor.Destroy(suiteName);
            }
            suiteDescriptors.Clear();
            Log.For("EdiTestMachinery").InfoFormat("App domain cleanup is finished");
        }

        private static bool appDomainIsIntialized;
        private static readonly HashSet<Type> setUpedFixtures = new HashSet<Type>();
        private static readonly ConcurrentDictionary<string, SuiteDescriptor> suiteDescriptors = new ConcurrentDictionary<string, SuiteDescriptor>();

        private class SuiteDescriptor
        {
            public SuiteDescriptor([NotNull] Assembly testAssembly)
            {
                Order = order++;
                TestAssembly = testAssembly;
                LazyContainer = new Lazy<IContainer>(() => new Container(new ContainerConfiguration(AssembliesLoader.Load())));
                SuiteContext = new EdiTestSuiteContextData(LazyContainer);
                SetUpedSuiteWrappers = new List<EdiTestSuiteWrapperAttribute>();
            }

            public int Order { get; private set; }

            [NotNull]
            public Assembly TestAssembly { get; private set; }

            [NotNull]
            public Lazy<IContainer> LazyContainer { get; private set; }

            [NotNull]
            public EdiTestSuiteContextData SuiteContext { get; private set; }

            [NotNull]
            public List<EdiTestSuiteWrapperAttribute> SetUpedSuiteWrappers { get; private set; }

            public void Destroy([NotNull] string suiteName)
            {
                AggregateException error;
                if(!SuiteContext.TryDestroy(out error))
                    Log.For("EdiTestMachinery").Fatal(string.Format("Failed to destroy suite context for: {0}", suiteName), error);
                if(!LazyContainer.IsValueCreated)
                    return;
                try
                {
                    LazyContainer.Value.Dispose();
                }
                catch(Exception e)
                {
                    Log.For("EdiTestMachinery").Fatal(string.Format("Failed to dispose container for: {0}", suiteName), e);
                }
            }

            private static int order;
        }
    }
}