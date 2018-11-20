using System.Collections.Generic;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TopologicalSorting
{
    public class TopSortResult<T>
    {
        public TopSortResult()
        {
            SortedNodes = new List<DependencyNode<T>>();
            Cycles = new List<List<DependencyNode<T>>>();
        }

        [NotNull]
        public List<DependencyNode<T>> SortedNodes { get; }

        [NotNull]
        public List<List<DependencyNode<T>>> Cycles { get; }
    }
}