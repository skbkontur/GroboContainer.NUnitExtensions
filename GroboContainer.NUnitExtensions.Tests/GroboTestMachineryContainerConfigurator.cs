using System.Collections.Concurrent;
using System.Reflection;

using GroboContainer.Core;
using GroboContainer.Impl;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class GroboTestMachineryContainerConfigurator
    {
        [NotNull]
        public static ContainerConfiguration GetContainerConfiguration([NotNull] string testSuiteName)
        {
            return containerConfigurations.GetOrAdd(testSuiteName, _ => new ContainerConfiguration(new[] {Assembly.GetExecutingAssembly()}, testSuiteName, ContainerMode.UseShortLog));
        }

        private static readonly ConcurrentDictionary<string, ContainerConfiguration> containerConfigurations = new ConcurrentDictionary<string, ContainerConfiguration>();
    }
}