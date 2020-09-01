using FluentAssertions;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestSuite(suiteName)]
    [WithDebugLogPerSuite]
    public class GroboTestContext_In_SetUp_And_TearDown_Test
    {
        [GroboSetUp]
        public void SetUp()
        {
            GroboTestContext.Current.SuiteName().Should().Be(suiteName);
        }

        [Test]
        public void Test()
        {
            GroboTestContext.Current.SuiteName().Should().Be(suiteName);
        }

        [GroboTearDown]
        public void TearDown()
        {
            GroboTestContext.Current.SuiteName().Should().Be(suiteName);
        }

        private const string suiteName = "GroboTestContextInSetUpAndTearDown_Test";
    }
}