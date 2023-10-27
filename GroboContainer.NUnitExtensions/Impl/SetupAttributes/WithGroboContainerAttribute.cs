using System;
using System.Linq;
using System.Reflection;

using GroboContainer.Core;
using GroboContainer.Impl;
using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.SetupAttributes
{
    public class WithGroboContainerAttribute : TestSuiteWrapperAttribute
    {
        public const string ContainerKey = "LazyContainer";

        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            suiteContext.AddItem(ContainerKey, new Lazy<IContainer>(() => new Container(GetContainerConfiguration(suiteName, testAssembly))));
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            var lazyContainer = suiteContext.GetContextItem<Lazy<IContainer>>(ContainerKey);
            if (!lazyContainer.IsValueCreated)
                return;
            try
            {
                lazyContainer.Value.Dispose();
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
            var containerConfiguratorTypes = testAssembly.GetExportedTypes().Where(t => t.IsClass && t.Name == containerConfiguratorTypeName).ToList();
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
    }
}