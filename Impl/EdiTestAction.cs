using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using GroboContainer.Core;
using GroboContainer.Impl;

using JetBrains.Annotations;

using NUnit.Framework.Interfaces;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;
using SKBKontur.Catalogue.Objects;
using SKBKontur.Catalogue.ServiceLib;
using SKBKontur.Catalogue.ServiceLib.Logging;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl
{
    public static class EdiTestAction
    {
        public static void BeforeTest([NotNull] ITest testDetails)
        {
            EnsureAppDomainIntialization();

            var test = testDetails.Method.MethodInfo;
            test.EnsureNunitAttributesAbscence();
            var fixtureType = test.GetFixtureType();
            var testFixture = testDetails.Fixture;
            if (fixtureType != testFixture.GetType())
                throw new InvalidProgramStateException($"TestFixtureType mismatch for: {test.GetMethodName()}");

            var suiteName = test.GetSuiteName();
            var suiteDescriptor = suiteDescriptors.GetOrAdd(suiteName, x => new SuiteDescriptor(fixtureType.Assembly));
            var suiteContext = suiteDescriptor.SuiteContext;
            var suiteWrappers = test.GetSuiteWrappers();
            lock (suiteDescriptor)
            {
                foreach (var suiteWrapper in suiteWrappers)
                    if (suiteDescriptor.SetUpedSuiteWrappers.TryAdd(suiteWrapper, Timestamp.Now))
                        suiteWrapper.SetUp(suiteName, suiteDescriptor.TestAssembly, suiteContext);
            }

            if (IsFixtureNotSetuped(testFixture))
            {
                var fixtureSetUpMethod = test.FindFixtureSetUpMethod();
                if (fixtureSetUpMethod != null)
                {
                    if (suiteName != fixtureType.FullName)
                        throw new InvalidProgramStateException($"EdiTestFixtureSetUp method is only allowed inside EdiTestFixture suite. Test: {test.GetMethodName()}");
                    InvokeWrapperMethod(fixtureSetUpMethod, testFixture, suiteContext);
                }
                InjectFixtureFields(suiteContext, testFixture);
            }

            var testName = testDetails.FullName;
            var methodContext = new EdiTestMethodContextData(suiteDescriptor.LazyContainer);
            foreach (var methodWrapper in test.GetMethodWrappers())
                methodWrapper.SetUp(testName, suiteContext, methodContext);

            EdiTestContextHolder.SetCurrentContext(suiteContext, methodContext);

            InvokeWrapperMethod(test.FindSetUpMethod(), testFixture);
        }

        private static bool IsFixtureNotSetuped([NotNull] object testFixture)
        {
            lock (setUpedFixturesLock)
            {
                if (setUpedFixtures.TryGetValue(testFixture, out _))
                    return false;
                setUpedFixtures.Add(testFixture, null);
                return true;
            }
        }

        public static void AfterTest([NotNull] ITest testDetails)
        {
            var errors = new List<Exception>();
            var test = testDetails.Method.MethodInfo;
            var suiteName = test.GetSuiteName();
            if (!suiteDescriptors.TryGetValue(suiteName, out var suiteDescriptor))
                throw new InvalidProgramStateException($"Suite context is not set for: {suiteName}");

            try
            {
                InvokeWrapperMethod(test.FindTearDownMethod(), testDetails.Fixture);
            }
            catch (Exception exception)
            {
                errors.Add(exception);
            }

            var methodContext = EdiTestContextHolder.ResetCurrentTestContext();

            var testName = testDetails.FullName;
            foreach (var methodWrapper in Enumerable.Reverse(test.GetMethodWrappers()))
            {
                try
                {
                    methodWrapper.TearDown(testName, suiteDescriptor.SuiteContext, methodContext);
                }
                catch (Exception exception)
                {
                    errors.Add(exception);
                }
            }

            if (!methodContext.TryDestroy(out var error))
            {
                errors.Add(error);
            }
            if (errors.Count > 0)
            {
                if (errors.Count == 1)
                    throw errors[0];
                throw new AggregateException("After test methods failed.", errors);
            }
        }

        private static void InvokeWrapperMethod([CanBeNull] MethodInfo wrapperMethod, [NotNull] object testFixture, params object[] @params)
        {
            if (wrapperMethod == null)
                return;
            try
            {
                wrapperMethod.Invoke(testFixture, @params);
            }
            catch (TargetInvocationException exception)
            {
                exception.RethrowInnerException();
            }
        }

        private static void InjectFixtureFields([NotNull] EdiTestSuiteContextData suiteContext, [NotNull] object testFixture)
        {
            foreach (var fieldInfo in testFixture.GetType().GetFieldsForInjection())
                fieldInfo.SetValue(testFixture, suiteContext.Container.Get(fieldInfo.FieldType));
        }

        private static void EnsureAppDomainIntialization()
        {
            if (!appDomainIsIntialized)
            {
                lock (appDomainInitializationLock)
                {
                    if (!appDomainIsIntialized)
                    {
                        AppDomain.CurrentDomain.DomainUnload += (sender, args) => OnAppDomainUnload();
                        appDomainIsIntialized = true;
                    }
                }
            }
        }

        private static void OnAppDomainUnload()
        {
            var suiteDescriptorsInOrderOfDestruction = suiteDescriptors.OrderByDescending(x => x.Value.Timestamp).ToList();
            Log.For("EdiTestMachinery").InfoFormat("Suites to tear down: {0}", string.Join(", ", suiteDescriptorsInOrderOfDestruction.Select(x => x.Key)));
            foreach (var kvp in suiteDescriptorsInOrderOfDestruction)
            {
                var suiteName = kvp.Key;
                var suiteDescriptor = kvp.Value;
                foreach (var suiteWrapper in suiteDescriptor.SetUpedSuiteWrappers.OrderByDescending(x => x.Value).Select(x => x.Key))
                    suiteWrapper.TearDown(suiteName, suiteDescriptor.TestAssembly, suiteDescriptor.SuiteContext);
                suiteDescriptor.Destroy(suiteName);
            }
            suiteDescriptors.Clear();
            Log.For("EdiTestMachinery").InfoFormat("App domain cleanup is finished");
        }

        private static bool appDomainIsIntialized;
        private static readonly object appDomainInitializationLock = new object();
        private static readonly object setUpedFixturesLock = new object();
        private static readonly ConditionalWeakTable<object, object> setUpedFixtures = new ConditionalWeakTable<object, object>();
        private static readonly ConcurrentDictionary<string, SuiteDescriptor> suiteDescriptors = new ConcurrentDictionary<string, SuiteDescriptor>();

        private class SuiteDescriptor
        {
            public SuiteDescriptor([NotNull] Assembly testAssembly)
            {
                Timestamp = Timestamp.Now;
                TestAssembly = testAssembly;
                LazyContainer = new Lazy<IContainer>(() => new Container(new ContainerConfiguration(AssembliesLoader.Load(), "test", ContainerMode.UseShortLog)));
                SuiteContext = new EdiTestSuiteContextData(LazyContainer);
                SetUpedSuiteWrappers = new ConcurrentDictionary<EdiTestSuiteWrapperAttribute, Timestamp>();
            }

            public Timestamp Timestamp { get; }

            [NotNull]
            public Assembly TestAssembly { get; }

            [NotNull]
            public Lazy<IContainer> LazyContainer { get; }

            [NotNull]
            public EdiTestSuiteContextData SuiteContext { get; }

            [NotNull]
            public ConcurrentDictionary<EdiTestSuiteWrapperAttribute, Timestamp> SetUpedSuiteWrappers { get; }

            public void Destroy([NotNull] string suiteName)
            {
                if (!SuiteContext.TryDestroy(out var error))
                    Log.For("EdiTestMachinery").Fatal($"Failed to destroy suite context for: {suiteName}", error);
                if (!LazyContainer.IsValueCreated)
                    return;
                try
                {
                    LazyContainer.Value.Dispose();
                }
                catch (Exception e)
                {
                    Log.For("EdiTestMachinery").Fatal($"Failed to dispose container for: {suiteName}", e);
                }
            }
        }
    }
}