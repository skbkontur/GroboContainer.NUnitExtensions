using System.Reflection;
using System.Threading;

using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Parallel
{
    public class WithFirstFieldLongInitialization : EdiTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            Thread.Sleep(1000);
            suiteContext.AddItem("item1", new object());
        }
    }

    [WithFirstFieldLongInitialization]
    public class WithSecondFieldDependingOnFirstInitialization : EdiTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            var item1 = suiteContext.GetContextItem<object>("item1");
            suiteContext.AddItem("item2", new[] {item1, new object()});
        }
    }

    [EdiTestSuite("NoLockSuite"), WithSecondFieldDependingOnFirstInitialization, Parallelizable(ParallelScope.Self)]
    public class SeveralWrappersFirstTest
    {
        [Test]
        public void Test()
        {
            EdiTestContext.Current.TryGetContextItem("item1", out var item1).Should().BeTrue();
            EdiTestContext.Current.TryGetContextItem("item2", out var item2).Should().BeTrue();
            var array = (object[])item2;
            array[0].Should().Be(item1);
        }
    }

    [EdiTestSuite("NoLockSuite"), WithSecondFieldDependingOnFirstInitialization, Parallelizable(ParallelScope.Self)]
    public class SeveralWrappersSecondTest
    {
        [Test]
        public void Test()
        {
            EdiTestContext.Current.TryGetContextItem("item1", out var item1).Should().BeTrue();
            EdiTestContext.Current.TryGetContextItem("item2", out var item2).Should().BeTrue();
            var array = (object[])item2;
            array[0].Should().Be(item1);
        }
    }
}