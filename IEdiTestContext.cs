using GroboContainer.Core;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public interface IEdiTestContext
    {
        [NotNull]
        IContainer Container { get; }

        [CanBeNull]
        TItem TryGetContextItem<TItem>([NotNull] string contextItemName);
    }
}