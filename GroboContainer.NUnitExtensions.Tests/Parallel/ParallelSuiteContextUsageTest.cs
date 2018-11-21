using System.Reflection;

using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Parallel
{
    public class WithFirstContextItem : GroboTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            suiteContext.AddItem("item1", new object());
        }
    }

    public class WithSecondContextItem : GroboTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            suiteContext.AddItem("item2", new object());
        }
    }

    [GroboTestSuite("ParallelUsage"), WithFirstContextItem, Parallelizable(ParallelScope.Self)]
    public class FirstParallelSuiteContextUsageTest
    {
        [Test]
        public void Test()
        {
            GroboTestContext.Current.TryGetContextItem("item1", out _).Should().BeTrue();
        }
    }

    [GroboTestSuite("ParallelUsage"), WithSecondContextItem, Parallelizable(ParallelScope.Self)]
    public class SecondParallelSuiteContextUsageTest
    {
        [Test]
        public void Test()
        {
            GroboTestContext.Current.TryGetContextItem("item2", out _).Should().BeTrue();
        }
    }
}