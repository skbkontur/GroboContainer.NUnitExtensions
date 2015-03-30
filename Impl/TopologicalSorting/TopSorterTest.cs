using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting
{
    [TestFixture]
    public class TopSorterTest
    {
        [Test]
        public void Test1()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            n1.DependsOn(n2);
            n2.DependsOn(n1);

            var sortResult = RunTopSorter(new[] { n1, n2 });
            Assert.AreEqual(sortResult.Cycles[0][0], n2);
            Assert.AreEqual(sortResult.Cycles[0][1], n1);
            Assert.AreEqual(sortResult.Cycles[0].Count, 2);
            Assert.AreEqual(sortResult.Cycles.Count, 1);
            Assert.AreEqual(sortResult.SortedNodes.Count, 0);
        }

        [Test]
        public void Test2()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            var n3 = Node.Create("n3");
            n1.DependsOn(n2);
            n2.DependsOn(n3);

            var sortResult = RunTopSorter(new[] { n1, n2, n3 });
            Assert.AreEqual(sortResult.Cycles.Count, 0);
            Assert.AreEqual(sortResult.SortedNodes[0], n3);
            Assert.AreEqual(sortResult.SortedNodes[1], n2);
            Assert.AreEqual(sortResult.SortedNodes[2], n1);
            Assert.AreEqual(sortResult.SortedNodes.Count, 3);
        }

        [Test]
        public void Test3()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            var n3 = Node.Create("n3");
            n1.DependsOn(n2);
            n2.DependsOn(n3);

            var sortResult = RunTopSorter(new[] { n3, n2, n1 });
            Assert.AreEqual(sortResult.Cycles.Count, 0);
            CheckDependencies(sortResult.SortedNodes);
            Assert.AreEqual(sortResult.SortedNodes.Count, 3);
        }

        [Test]
        public void CycleTest()
        {
            var n1 = Node.Create("1");
            var n2 = Node.Create("2");
            var n3 = Node.Create("3");
            n2.DependsOn(n1);
            n3.DependsOn(n2);
            n1.DependsOn(n3);
            var result = RunTopSorter(new[] { n1, n2, n3 });
            Assert.That(result.Cycles.Single(), Is.EqualTo(new[] {n2, n3, n1}));
        }

        [Test]
        public void WikipediaTest()
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
            CheckDependencies(result.SortedNodes);
        }

        [Test]
        public void Test4()
        {
            var n1 = Node.Create("n1");
            var n2 = Node.Create("n2");
            var n3 = Node.Create("n3");
            var n4 = Node.Create("n4");
            var n5 = Node.Create("n5");
            n2.DependsOn(n1);
            n2.DependsOn(n3);
            n4.DependsOn(n3);
            n4.DependsOn(n5);

            var sortResult = RunTopSorter(new[] { n1, n2, n3, n4, n5 });
            CheckDependencies(sortResult.SortedNodes);
        }

        [Test]
        public void Test5()
        {
            for(int i = 0; i < 100; i++)
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

                var testDependencyNodes = new[] {n1, n2, n3, n4, n5, n6, n7, n8, n9};
                testDependencyNodes = SortRandomly(testDependencyNodes).ToArray();
                var sortResult = RunTopSorter(testDependencyNodes);
                var sortedNodes = sortResult.SortedNodes;
                Assert.AreEqual(9, sortedNodes.Count);
                CheckDependencies(sortedNodes);
            }
        }

        public static TopSortResult<string> RunTopSorter(ICollection<DependencyNode<string>> nodes)
        {
            return new TopSorter<string>().Run(nodes);
        }

        private static IEnumerable<T> SortRandomly<T>(IEnumerable<T> enumerable)
        {
            var random = new Random();
            var list = enumerable.Select(t => new KeyValuePair<T, int>(t, random.Next())).ToList();
            return list.OrderBy(i => i.Value).Select(t => t.Key);
        }

        private static void CheckDependencies<T>(List<DependencyNode<T>> nodes)
        {
            Console.Out.WriteLine(string.Join(", ", nodes));
            var loaded = new Dictionary<DependencyNode<T>, bool>();
            foreach(var node in nodes)
            {
                foreach(var n in node.GetDependencies())
                    Assert.IsTrue(loaded[n]);
                loaded[node] = true;
            }
        }
    }
}