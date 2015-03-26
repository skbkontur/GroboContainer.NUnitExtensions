using GroboContainer.Core;
using GroboContainer.Impl;

using JetBrains.Annotations;

using SKBKontur.Catalogue.ServiceLib;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public static class EdiTestContextDataExtensions
    {
        [NotNull]
        public static IContainer GetContainer([NotNull] this IEdiTestContextData contextData)
        {
            return (IContainer)contextData.GetItem(ContainerItemKey);
        }

        public static void InitContainer([NotNull] this IEdiTestContextData contextData)
        {
            contextData.AddItem(ContainerItemKey, new Container(new ContainerConfiguration(AssembliesLoader.Load())));
        }

        public const string ContainerItemKey = "__Container";
    }
}