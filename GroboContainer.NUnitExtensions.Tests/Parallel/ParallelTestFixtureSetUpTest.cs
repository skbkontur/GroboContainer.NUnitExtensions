using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Parallel
{
    [GroboTestFixture, Parallelizable(ParallelScope.Self)]
    public class FirstTestFixtureSetUpTest
    {
        [GroboTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableGroboTestContext suiteContext)
        {
            suiteContext.AddItem("item1", new object());
        }

        [Test]
        public void Test()
        {
            GroboTestContext.Current.TryGetContextItem("item1", out _).Should().BeTrue();
        }
    }

    [GroboTestFixture, Parallelizable(ParallelScope.Self)]
    public class SecondTestFixtureSetUpTest
    {
        [GroboTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableGroboTestContext suiteContext)
        {
            suiteContext.AddItem("item2", new object());
        }

        [Test]
        public void Test()
        {
            GroboTestContext.Current.TryGetContextItem("item2", out _).Should().BeTrue();
        }
    }

    [GroboTestFixture, Parallelizable(ParallelScope.Children)]
    public class ThirdTestFixtureSetUpTest
    {
        [GroboTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableGroboTestContext suiteContext)
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
            GroboTestContext.Current.TryGetContextItem("item3", out var item3).Should().BeTrue();
            ((Counter)item3).InvocationsCount.Should().Be(0);
        }
    }
}