using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TopologicalSorting
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