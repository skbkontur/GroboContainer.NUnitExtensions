using System.Threading;

using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Parallel
{
    [UsedImplicitly]
    public class InjectedField
    {
    }

    [EdiTestFixture, Parallelizable(ParallelScope.Children)]
    public class LongTestFixtureSetUpTest
    {
        [EdiTestFixtureSetUp]
        public void SetUp(IEditableEdiTestContext suiteContext)
        {
            Thread.Sleep(1000);
        }

        [Test]
        public void Test1()
        {
            injected.Should().NotBeNull();
        }

        [Test]
        public void Test2()
        {
            injected.Should().NotBeNull();
        }

#pragma warning disable 649
        [Injected]
        private readonly InjectedField injected;
#pragma warning restore 649
    }
}