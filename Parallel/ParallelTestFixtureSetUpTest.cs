using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Parallel
{
    [EdiTestFixture, Parallelizable(ParallelScope.Self)]
    public class FirstTestFixtureSetUpTest
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableEdiTestContext suiteContext)
        {
            suiteContext.AddItem("item1", new object());
        }

        [Test]
        public void Test()
        {
            EdiTestContext.Current.TryGetContextItem("item1", out _).Should().BeTrue();
        }
    }

    [EdiTestFixture, Parallelizable(ParallelScope.Self)]
    public class SecondTestFixtureSetUpTest
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableEdiTestContext suiteContext)
        {
            suiteContext.AddItem("item2", new object());
        }

        [Test]
        public void Test()
        {
            EdiTestContext.Current.TryGetContextItem("item2", out _).Should().BeTrue();
        }
    }

    [EdiTestFixture, Parallelizable(ParallelScope.Children)]
    public class ThirdTestFixtureSetUpTest
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableEdiTestContext suiteContext)
        {
            if (suiteContext.TryGetContextItem("item3", out var item3))
            {
                ((Counter)item3).InvocationsCount++;
            }
            suiteContext.AddItem("item3", new Counter());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public void Test(int n)
        {
            EdiTestContext.Current.TryGetContextItem("item3", out var item3).Should().BeTrue();
            ((Counter)item3).InvocationsCount.Should().Be(0);
        }
    }
}