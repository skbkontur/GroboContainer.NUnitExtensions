using System.Collections.Generic;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    public class DependencyNode<T>
    {
        public DependencyNode([NotNull] T payload)
        {
            Payload = payload;
            dependencies = new List<DependencyNode<T>>();
        }

        [NotNull]
        public T Payload { get; private set; }

        public bool InResults { get; set; }
        public bool InProgress { get; set; }

        public void DependsOn([NotNull] params DependencyNode<T>[] nodes)
        {
            foreach (var node in nodes)
                dependencies.Add(node);
        }

        [NotNull]
        public IEnumerable<DependencyNode<T>> GetDependencies()
        {
            return dependencies;
        }

        public override string ToString()
        {
            return Payload.ToString();
        }

        private readonly List<DependencyNode<T>> dependencies;
    }
}