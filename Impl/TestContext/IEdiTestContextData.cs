using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public interface IEdiTestContextData
    {
        void AddItem([NotNull] string itemName, [NotNull] object itemValue);

        [NotNull]
        object GetItem([NotNull] string itemName);

        bool TryGetItem([NotNull] string itemName, out object itemValue);

        bool RemoveItem([NotNull] string itemName);
    }
}