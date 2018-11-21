using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TestContext
{
    public interface IEditableGroboTestContext : IGroboTestContext
    {
        void AddItem([NotNull] string itemName, [NotNull] object itemValue);
        bool RemoveItem([NotNull] string itemName);
    }
}