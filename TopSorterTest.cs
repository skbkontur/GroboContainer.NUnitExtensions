using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery
{
    [TestFixture]
    public class TopSorterTest
    {
        [Test]
        public void Loop()
        {
            var n1 = Node.Create("n1");
            n1.DependsOn(n1);
            var result = RunTopSorter(new[] { n1 });
            Assert.That(result.Cycles.Single(), Is.EqualTo(new[] { n1 }));
            AssertTopologicalOrder(result.SortedNodes);
        }

        [Test]
        public void Line()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            var n3 = Node.Create("n3");
            n1.DependsOn(n2);
            n2.DependsOn(n3);
            var result = RunTopSorter(new[] { n1, n2, n3 });
            Assert.That(result.SortedNodes, Is.EqualTo(new[] { n3, n2, n1 }));
            AssertTopologicalOrder(result.SortedNodes);
        }

        [Test]
        public void Cycle_2Nodes()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            n1.DependsOn(n2);
            n2.DependsOn(n1);
            var result = RunTopSorter(new[] { n1, n2 });
            Assert.That(result.Cycles.Single(), Is.EqualTo(new[] { n2, n1 }));
        }

        [Test]
        public void Cycle_3Nodes()
        {
            var n1 = Node.Create("1");
            var n2 = Node.Create("2");
            var n3 = Node.Create("3");
            n2.DependsOn(n1);
            n3.DependsOn(n2);
            n1.DependsOn(n3);
            var result = RunTopSorter(new[] { n1, n2, n3 });
            Assert.That(result.Cycles.Single(), Is.EqualTo(new[] { n2, n3, n1 }));
        }

        [Test]
        public void CycleAndRoot()
        {
            var n1 = Node.Create("1");
            var n2 = Node.Create("2");
            var n3 = Node.Create("3");
            var n4 = Node.Create("4");
            n2.DependsOn(n1);
            n3.DependsOn(n2);
            n1.DependsOn(n3);
            n4.DependsOn(n2);
            var result = RunTopSorter(new[] { n4, n1, n2, n3 });
            Assert.That(result.Cycles.Single(), Is.EqualTo(new[] { n3, n1, n2, n4 }));
            Assert.That(result.SortedNodes, Is.Empty);
        }

        [Test]
        public void TwoCyclesAndBridge()
        {
            var n1 = Node.Create("1");
            var n2 = Node.Create("2");
            var n3 = Node.Create("3");
            var n4 = Node.Create("4");
            var n5 = Node.Create("5");
            var n6 = Node.Create("6");
            n2.DependsOn(n1);
            n3.DependsOn(n2);
            n1.DependsOn(n3);
            n5.DependsOn(n4);
            n6.DependsOn(n5);
            n4.DependsOn(n6);
            n2.DependsOn(n6);
            var result = RunTopSorter(new[] { n1, n2, n3, n4, n5, n6 });
            Assert.That(result.Cycles.Count, Is.EqualTo(2));
            Assert.That(result.Cycles[0], Is.EqualTo(new[] { n2, n3, n1 }));
            Assert.That(result.Cycles[1], Is.EqualTo(new[] { n5, n6, n4 }));
            Assert.That(result.SortedNodes, Is.Empty);
        }

        [Test]
        public void TwoCyclesAndRoot()
        {
            var n1 = Node.Create("1");
            var n2 = Node.Create("2");
            var n3 = Node.Create("3");
            var n4 = Node.Create("4");
            var n5 = Node.Create("5");
            var n6 = Node.Create("6");
            var n7 = Node.Create("7");
            n2.DependsOn(n1);
            n3.DependsOn(n2);
            n1.DependsOn(n3);
            n5.DependsOn(n4);
            n6.DependsOn(n5);
            n4.DependsOn(n6);
            n7.DependsOn(n2);
            n7.DependsOn(n5);
            var result = RunTopSorter(new[] { n1, n2, n3, n7, n4, n5, n6 });
            Assert.That(result.Cycles.Count, Is.EqualTo(2));
            Assert.That(result.Cycles[0], Is.EqualTo(new[] { n2, n3, n1 }));
            Assert.That(result.Cycles[1], Is.EqualTo(new[] { n6, n4, n5, n7 }));
            Assert.That(result.SortedNodes, Is.Empty);
        }

        [Test]
        public void SeveralConnectedComponents()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            n1.DependsOn(n2);
            n2.DependsOn(n1);
            var n3 = Node.Create("n3");
            var n4 = Node.Create("n4");
            var n5 = Node.Create("n5");
            var n6 = Node.Create("n6");
            n3.DependsOn(n4);
            n3.DependsOn(n5);
            n4.DependsOn(n6);
            n5.DependsOn(n6);
            var n7 = Node.Create("n7");
            var n8 = Node.Create("n8");
            var n9 = Node.Create("n9");
            n7.DependsOn(n8);
            n8.DependsOn(n9);
            var result = RunTopSorter(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9 });
            Assert.That(result.Cycles.Single(), Is.EqualTo(new[] { n2, n1 }));
            Assert.That(result.SortedNodes, Is.EqualTo(new[] { n6, n4, n5, n3, n9, n8, n7 }));
        }

        [Test]
        public void SampleFromWikipedia()
        {
            var n7 = Node.Create("7");
            var n5 = Node.Create("5");
            var n3 = Node.Create("3");
            var n11 = Node.Create("11");
            var n8 = Node.Create("8");
            var n2 = Node.Create("2");
            var n9 = Node.Create("9");
            var n10 = Node.Create("10");
            n7.DependsOn(n11, n8);
            n5.DependsOn(n11);
            n3.DependsOn(n8);
            n11.DependsOn(n2, n9, n10);
            n8.DependsOn(n9);

            var result = RunTopSorter(new[] { n2, n3, n5, n7, n8, n9, n10, n11 });
            Assert.That(result.Cycles, Is.Empty);
            Assert.That(result.SortedNodes, Is.EqualTo(new[] {n2, n9, n8, n3, n10, n11, n5, n7}));
            AssertTopologicalOrder(result.SortedNodes);
        }

        [Test]
        [Repeat(100)]
        public void RandomInitialOrder()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            var n3 = Node.Create("n3");
            var n4 = Node.Create("n4");
            var n5 = Node.Create("n5");
            var n6 = Node.Create("n6");
            var n7 = Node.Create("n7");
            var n8 = Node.Create("n8");
            var n9 = Node.Create("n9");
            n1.DependsOn(n4);
            n1.DependsOn(n5);
            n2.DependsOn(n4);
            n2.DependsOn(n6);
            n3.DependsOn(n5);
            n3.DependsOn(n7);
            n4.DependsOn(n7);
            n4.DependsOn(n8);
            n5.DependsOn(n7);
            n5.DependsOn(n9);
            n6.DependsOn(n8);
            n6.DependsOn(n9);

            var nodes = SortRandomly(n1, n2, n3, n4, n5, n6, n7, n8, n9).ToArray();
            var result = RunTopSorter(nodes);
            Assert.That(result.SortedNodes.Count, Is.EqualTo(9));
            AssertTopologicalOrder(result.SortedNodes);
        }

        private static TopSortResult<string> RunTopSorter(ICollection<DependencyNode<string>> nodes)
        {
            var result = new TopSorter<string>().Run(nodes);
            AssertAllNodesAreProcessed(result, nodes.Count);
            return result;
        }

        private T[] SortRandomly<T>(params T[] items)
        {
            var list = items.Select(t => new KeyValuePair<T, int>(t, random.Next())).ToList();
            return list.OrderBy(i => i.Value).Select(t => t.Key).ToArray();
        }

        private static void AssertTopologicalOrder<T>(List<DependencyNode<T>> nodes)
        {
            Console.Out.WriteLine(string.Join(", ", nodes));
            var visited = new Dictionary<DependencyNode<T>, bool>();
            foreach(var node in nodes)
            {
                foreach(var n in node.GetDependencies())
                    Assert.IsTrue(visited[n]);
                visited[node] = true;
            }
        }

        private static void AssertAllNodesAreProcessed(TopSortResult<string> result, int expectedTotalNodesCount)
        {
            var distinctPayloads = result.SortedNodes.Select(x => x.Payload).Concat(result.Cycles.SelectMany(x => x.Select(y => y.Payload))).Distinct().ToList();
            Assert.That(distinctPayloads.Count, Is.EqualTo(expectedTotalNodesCount));
        }

        private readonly Random random = new Random();
    }
}