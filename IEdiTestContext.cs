using GroboContainer.Core;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public interface IEdiTestContext
    {
        [NotNull]
        IContainer Container { get; }

        bool TryGetContextItem([NotNull] string itemName, out object itemValue);
    }
}