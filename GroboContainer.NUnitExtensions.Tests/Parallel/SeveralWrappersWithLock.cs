using System.Reflection;
using System.Threading;

using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Parallel
{
    public class WithFirstFieldLongInitialization : GroboTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            Thread.Sleep(1000);
            suiteContext.AddItem("item1", new object());
        }
    }

    [WithFirstFieldLongInitialization]
    public class WithSecondFieldDependingOnFirstInitialization : GroboTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            var item1 = suiteContext.GetContextItem<object>("item1");
            suiteContext.AddItem("item2", new[] {item1, new object()});
        }
    }

    [GroboTestSuite("NoLockSuite"), WithSecondFieldDependingOnFirstInitialization, Parallelizable(ParallelScope.Self)]
    public class SeveralWrappersFirstTest
    {
        [Test]
        public void Test()
        {
            GroboTestContext.Current.TryGetContextItem("item1", out var item1).Should().BeTrue();
            GroboTestContext.Current.TryGetContextItem("item2", out var item2).Should().BeTrue();
            var array = (object[])item2;
            array[0].Should().Be(item1);
        }
    }

    [GroboTestSuite("NoLockSuite"), WithSecondFieldDependingOnFirstInitialization, Parallelizable(ParallelScope.Self)]
    public class SeveralWrappersSecondTest
    {
        [Test]
        public void Test()
        {
            GroboTestContext.Current.TryGetContextItem("item1", out var item1).Should().BeTrue();
            GroboTestContext.Current.TryGetContextItem("item2", out var item2).Should().BeTrue();
            var array = (object[])item2;
            array[0].Should().Be(item1);
        }
    }
}