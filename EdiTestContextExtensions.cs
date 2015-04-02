using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public static class EdiTestContextExtensions
    {
        [NotNull]
        public static TItem GetContextItem<TItem>([NotNull] this IEdiTestContext ctx, [NotNull] string contextItemName)
        {
            var contextItem = ctx.TryGetContextItem<TItem>(contextItemName);
            if(contextItem == null)
                throw new InvalidProgramStateException(string.Format("{0} is not set in context: {1}", contextItemName, ctx));
            return contextItem;
        }
    }
}