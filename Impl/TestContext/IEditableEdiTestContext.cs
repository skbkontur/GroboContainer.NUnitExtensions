using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public interface IEditableEdiTestContext : IEdiTestContext
    {
        void AddItem([NotNull] string itemName, [NotNull] object itemValue);
        bool RemoveItem([NotNull] string itemName);
    }
}