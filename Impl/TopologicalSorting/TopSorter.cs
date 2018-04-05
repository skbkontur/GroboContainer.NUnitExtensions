using System.Collections.Generic;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    public class TopSorter<T>
    {
        [NotNull]
        public TopSortResult<T> Run([NotNull] ICollection<DependencyNode<T>> nodes)
        {
            foreach (var node in nodes)
            {
                currentCycle = null;
                Process(node);
            }
            return result;
        }

        private void Process([NotNull] DependencyNode<T> node)
        {
            if (node.InResults)
                return;
            if (node.InProgress)
            {
                currentCycle = new List<DependencyNode<T>>();
                result.Cycles.Add(currentCycle);
                node.InResults = true;
                return;
            }
            node.InProgress = true;
            foreach (var dependentNode in node.GetDependencies())
            {
                Process(dependentNode);
                if (currentCycle != null)
                {
                    node.InResults = true;
                    currentCycle.Add(node);
                    return;
                }
            }
            node.InProgress = false;
            node.InResults = true;
            result.SortedNodes.Add(node);
        }

        private List<DependencyNode<T>> currentCycle = new List<DependencyNode<T>>();
        private readonly TopSortResult<T> result = new TopSortResult<T>();
    }
}