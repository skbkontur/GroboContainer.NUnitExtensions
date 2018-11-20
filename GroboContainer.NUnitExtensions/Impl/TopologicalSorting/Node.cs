using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    public static class Node
    {
        [NotNull]
        public static DependencyNode<T> Create<T>([NotNull] T payload)
        {
            return new DependencyNode<T>(payload);
        }
    }
}