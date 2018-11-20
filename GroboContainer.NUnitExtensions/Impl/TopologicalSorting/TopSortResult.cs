using System.Collections.Generic;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    public class TopSortResult<T>
    {
        public TopSortResult()
        {
            SortedNodes = new List<DependencyNode<T>>();
            Cycles = new List<List<DependencyNode<T>>>();
        }

        [NotNull]
        public List<DependencyNode<T>> SortedNodes { get; private set; }

        [NotNull]
        public List<List<DependencyNode<T>>> Cycles { get; private set; }
    }
}