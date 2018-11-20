using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TopologicalSorting
{
    public static class DependencyNodeCollectionExtensions
    {
        [NotNull]
        public static List<DependencyNode<T>> OrderTopologically<T>([NotNull] this ICollection<DependencyNode<T>> nodes)
        {
            var result = new TopSorter<T>().Run(nodes);
            if (result.Cycles.Any())
                throw new InvalidOperationException($"At least one cycle was detected in: {string.Join(", ", nodes)}; First cycle: {string.Join(", ", result.Cycles.First())}");
            return result.SortedNodes;
        }
    }
}