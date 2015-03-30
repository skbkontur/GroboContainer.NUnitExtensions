using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    public static class DependencyNodeCollectionExtensions
    {
        [NotNull]
        public static List<DependencyNode<T>> OrderTopologically<T>([NotNull] this ICollection<DependencyNode<T>> nodes)
        {
            var result = new TopSorter<T>().Run(nodes);
            if (result.Cycles.Any())
                throw new InvalidProgramStateException(string.Format("At least one cycle was detected in: {0}", string.Join(", ", nodes)));
            return result.SortedNodes;
        }
    }
}