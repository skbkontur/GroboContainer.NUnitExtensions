using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using GroboContainer.Core;
using GroboContainer.Impl;
using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

using NUnit.Framework.Interfaces;

namespace GroboContainer.NUnitExtensions.Impl
{
    public static class GroboTestAction
    {
        public static void BeforeTest([NotNull] ITest testDetails)
        {
            EnsureAppDomainInitialization();

            var test = testDetails.Method.MethodInfo;
            test.EnsureNunitAttributesAbsence();
            var fixtureType = test.GetFixtureType();
            var testFixture = testDetails.Fixture;
            if (fixtureType != testFixture.GetType())
                throw new InvalidOperationException($"TestFixtureType mismatch for: {test.GetMethodName()}");

            var suiteName = test.GetSuiteName();
            var suiteDescriptor = suiteDescriptors.GetOrAdd(suiteName, x => new SuiteDescriptor(suiteName, fixtureType.Assembly));
            var suiteContext = suiteDescriptor.SuiteContext;
            var methodContext = new GroboTestMethodContextData(suiteDescriptor.LazyContainer);

            GroboTestContextHolder.SetCurrentContext(suiteContext, methodContext);

            var suiteWrappers = test.GetSuiteWrappers();
            lock (suiteDescriptor)
            {
                foreach (var suiteWrapper in suiteWrappers)
                {
                    if (suiteDescriptor.SetUpedSuiteWrappers.Contains(suiteWrapper))
                        continue;
                    suiteWrapper.SetUp(suiteName, suiteDescriptor.TestAssembly, suiteContext);
                    suiteDescriptor.SetUpedSuiteWrappers.Add(suiteWrapper);
                }
            }

            lock (testFixture)
            {
                if (IsFixtureNotSetuped(testFixture))
                {
                    var fixtureSetUpMethod = test.FindFixtureSetUpMethod();
                    if (fixtureSetUpMethod != null)
                    {
                        if (suiteName != fixtureType.FullName)
                            throw new InvalidOperationException($"GroboTestFixtureSetUp method is only allowed inside GroboTestFixture suite. Test: {test.GetMethodName()}");
                        InvokeWrapperMethod(fixtureSetUpMethod, testFixture, suiteContext);
                    }
                    InjectFixtureFields(suiteContext, testFixture);
                }
            }

            var testName = testDetails.FullName;
            foreach (var methodWrapper in test.GetMethodWrappers())
            {
                methodWrapper.SetUp(testName, suiteContext, methodContext);
                methodContext.SetUppedMethodWrappers.Add(methodWrapper);
            }

            InvokeWrapperMethod(test.FindSetUpMethod(), testFixture);
            methodContext.IsSetUpped = true;
        }

        private static bool IsFixtureNotSetuped([NotNull] object testFixture)
        {
            if (setUpedFixtures.TryGetValue(testFixture, out _))
                return false;
            setUpedFixtures.Add(testFixture, null);
            return true;
        }

        public static void AfterTest([NotNull] ITest testDetails)
        {
            var errors = new List<Exception>();

            var test = testDetails.Method.MethodInfo;
            if (test.HasNunitAttributes())
                return;

            var suiteName = test.GetSuiteName();
            if (!suiteDescriptors.TryGetValue(suiteName, out var suiteDescriptor))
                throw new InvalidOperationException($"Suite context is not set for: {suiteName}");

            var methodContext = GroboTestContextHolder.ResetCurrentTestContext();
            if (methodContext.IsSetUpped)
            {
                try
                {
                    InvokeWrapperMethod(test.FindTearDownMethod(), testDetails.Fixture);
                }
                catch (Exception exception)
                {
                    errors.Add(exception);
                }
            }

            var testName = testDetails.FullName;
            foreach (var methodWrapper in Enumerable.Reverse(test.GetMethodWrappers()))
            {
                if (!methodContext.SetUppedMethodWrappers.Contains(methodWrapper))
                    continue;

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

        private static void InjectFixtureFields([NotNull] GroboTestSuiteContextData suiteContext, [NotNull] object testFixture)
        {
            foreach (var fieldInfo in testFixture.GetType().GetFieldsForInjection())
                fieldInfo.SetValue(testFixture, InstantiateField(suiteContext, fieldInfo));
        }

        private static object InstantiateField([NotNull] GroboTestSuiteContextData suiteContext, [NotNull] FieldInfo fieldInfo)
        {
            var fieldType = fieldInfo.FieldType;
            if (fieldType.IsGenericType)
            {
                var genericFieldType = fieldType.GetGenericTypeDefinition();
                if (supportedFactoryFuncTypes.Contains(genericFieldType))
                    return suiteContext.Container.GetCreationFunc(fieldType);
            }
            return suiteContext.Container.Get(fieldType);
        }

        private static void EnsureAppDomainInitialization()
        {
            if (!appDomainIsInitialized)
            {
                lock (appDomainInitializationLock)
                {
                    if (!appDomainIsInitialized)
                    {
                        AppDomain.CurrentDomain.DomainUnload += (sender, args) => OnAppDomainUnload();
                        appDomainIsInitialized = true;
                    }
                }
            }
        }

        private static void OnAppDomainUnload()
        {
            var suiteDescriptorsInOrderOfDestruction = suiteDescriptors.OrderByDescending(x => x.Value.Order).ToList();
            foreach (var kvp in suiteDescriptorsInOrderOfDestruction)
            {
                var suiteName = kvp.Key;
                var suiteDescriptor = kvp.Value;
                foreach (var suiteWrapper in Enumerable.Reverse(suiteDescriptor.SetUpedSuiteWrappers))
                    suiteWrapper.TearDown(suiteName, suiteDescriptor.TestAssembly, suiteDescriptor.SuiteContext);
                suiteDescriptor.Destroy(suiteName);
            }
            suiteDescriptors.Clear();
        }

        private static bool appDomainIsInitialized;
        private static readonly object appDomainInitializationLock = new object();
        private static readonly ConditionalWeakTable<object, object> setUpedFixtures = new ConditionalWeakTable<object, object>();
        private static readonly ConcurrentDictionary<string, SuiteDescriptor> suiteDescriptors = new ConcurrentDictionary<string, SuiteDescriptor>();
        private static readonly Type[] supportedFactoryFuncTypes = {typeof(Func<>), typeof(Func<,>), typeof(Func<,,>), typeof(Func<,,,>), typeof(Func<,,,,>)};

        private class SuiteDescriptor
        {
            public SuiteDescriptor([NotNull] string suiteName, [NotNull] Assembly testAssembly)
            {
                Order = Interlocked.Increment(ref order);
                TestAssembly = testAssembly;
                LazyContainer = new Lazy<IContainer>(() => new Container(GetContainerConfiguration(suiteName, testAssembly)));
                SuiteContext = new GroboTestSuiteContextData(LazyContainer);
                SetUpedSuiteWrappers = new List<GroboTestSuiteWrapperAttribute>();
            }

            public int Order { get; }

            [NotNull]
            public Assembly TestAssembly { get; }

            [NotNull]
            public Lazy<IContainer> LazyContainer { get; }

            [NotNull]
            public GroboTestSuiteContextData SuiteContext { get; }

            [NotNull]
            public List<GroboTestSuiteWrapperAttribute> SetUpedSuiteWrappers { get; }

            public void Destroy([NotNull] string suiteName)
            {
                if (!SuiteContext.TryDestroy(out var error))
                    Console.Error.WriteLine($"Failed to destroy suite context for {suiteName} with error: {error}");
                if (!LazyContainer.IsValueCreated)
                    return;
                try
                {
                    LazyContainer.Value.Dispose();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Failed to dispose container for {suiteName} with error: {e}");
                }
            }

            [NotNull]
            private static ContainerConfiguration GetContainerConfiguration([NotNull] string suiteName, [NotNull] Assembly testAssembly)
            {
                const string containerConfiguratorTypeName = "GroboTestMachineryContainerConfigurator";
                var containerConfiguratorTypes = testAssembly.GetExportedTypes().Where(t=> t.IsClass && t.Name == containerConfiguratorTypeName).ToList();
                if (!containerConfiguratorTypes.Any())
                    throw new InvalidOperationException($"Failed to get container configuration for test suite {suiteName}. There is no {containerConfiguratorTypeName} type in test assembly: {testAssembly}");
                if (containerConfiguratorTypes.Count > 1)
                    throw new InvalidOperationException($"Failed to get container configuration for test suite {suiteName}. There are multiple types with {containerConfiguratorTypeName} name in test assembly: {testAssembly}");

                const string getContainerConfigurationMethodName = "GetContainerConfiguration";
                var methodInfo = containerConfiguratorTypes.Single().GetMethod(getContainerConfigurationMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (methodInfo == null)
                    throw new InvalidOperationException($"Failed to get container configuration for test suite {suiteName}. There is no {containerConfiguratorTypeName}.{getContainerConfigurationMethodName}() method in test assembly: {testAssembly}");

                try
                {
                    return (ContainerConfiguration)methodInfo.Invoke(null, new object[] {suiteName});
                }
                catch (TargetInvocationException exception)
                {
                    exception.RethrowInnerException();
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return null;
                }
            }

            private static int order;
        }
    }
}