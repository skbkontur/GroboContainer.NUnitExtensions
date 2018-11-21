using System.Reflection;

using GroboContainer.Core;
using GroboContainer.Impl;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Tests
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class GroboTestMachineryContainerConfigurator
    {
        [NotNull]
        public static ContainerConfiguration GetContainerConfiguration([NotNull] string testSuiteName)
        {
            return new ContainerConfiguration(new[] {Assembly.GetExecutingAssembly()}, testSuiteName, ContainerMode.UseShortLog);
        }
    }
}