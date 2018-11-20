using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    public static class DependencyNodeCollectionExtensions
    {
        [NotNull]
        public static List<DependencyNode<T>> OrderTopologically<T>([NotNull] this ICollection<DependencyNode<T>> nodes)
        {
            var result = new TopSorter<T>().Run(nodes);
            if (result.Cycles.Any())
                throw new InvalidOperationException(string.Format("At least one cycle was detected in: {0}; First cycle: {1}", string.Join(", ", nodes), string.Join(", ", result.Cycles.First())));
            return result.SortedNodes;
        }
    }
}