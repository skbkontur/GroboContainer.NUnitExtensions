using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    public static class TopSort
    {
        [NotNull]
        public static List<DependencyNode<T>> RunAndThrowIfCycleIsDetected<T>([NotNull] ICollection<DependencyNode<T>> nodes)
        {
            var result = Run(nodes);
            if(result.Cycles.Any())
                throw new InvalidProgramStateException(string.Format("At least on cycle was detected in: {0}", string.Join(", ", nodes)));
            return result.SortedNodes;
        }

        [NotNull]
        public static TopSortResult<T> Run<T>([NotNull] ICollection<DependencyNode<T>> nodes)
        {
            return new TopSorter<T>().Run(nodes);
        }

        private class TopSorter<T>
        {
            [NotNull]
            public TopSortResult<T> Run([NotNull] ICollection<DependencyNode<T>> nodes)
            {
                foreach(var node in nodes)
                {
                    currentCycle = null;
                    Process(node);
                }
                return result;
            }

            private void Process([NotNull] DependencyNode<T> node)
            {
                if(node.InResults)
                    return;
                if(node.InProgress)
                {
                    currentCycle = new List<DependencyNode<T>>();
                    result.Cycles.Add(currentCycle);
                    node.InResults = true;
                    return;
                }
                node.InProgress = true;
                foreach(var dependentNode in node.GetDependencies())
                {
                    Process(dependentNode);
                    if(currentCycle != null)
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
}