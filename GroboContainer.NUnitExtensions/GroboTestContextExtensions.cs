using System;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    public static class GroboTestContextExtensions
    {
        [NotNull]
        public static TItem GetContextItem<TItem>([NotNull] this IGroboTestContext ctx, [NotNull] string itemName)
        {
            if (!ctx.TryGetContextItem(itemName, out var itemValue) || itemValue == null)
                throw new InvalidOperationException($"{itemName} is not set in context: {ctx}");
            return (TItem)itemValue;
        }
    }
}