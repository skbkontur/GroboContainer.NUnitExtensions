using System.Reflection;

using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Parallel
{
    public class WithFirstContextItem : EdiTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            suiteContext.AddItem("item1", new object());
        }
    }

    public class WithSecondContextItem : EdiTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            suiteContext.AddItem("item2", new object());
        }
    }

    [EdiTestSuite("ParallelUsage"), WithFirstContextItem, Parallelizable(ParallelScope.Self)]
    public class FirstParallelSuiteContextUsageTest
    {
        [Test]
        public void Test()
        {
            EdiTestContext.Current.TryGetContextItem("item1", out _).Should().BeTrue();
        }
    }

    [EdiTestSuite("ParallelUsage"), WithSecondContextItem, Parallelizable(ParallelScope.Self)]
    public class SecondParallelSuiteContextUsageTest
    {
        [Test]
        public void Test()
        {
            EdiTestContext.Current.TryGetContextItem("item2", out _).Should().BeTrue();
        }
    }
}