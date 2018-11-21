using FluentAssertions;

using GroboContainer.NUnitExtensions.Tests.ExecutionOrder;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionFailures
{
    [GroboTestSuite("BaseSuite")]
    [Explicit("Intentionally fails with 'There are multiple suite names (BaseSuite, BaseSuite) defined for ...' error")]
    public class DerivedTestClass_WithItsOwnGroboTestSuiteAttribute_ExplicitTest : TestBaseWithGroboTestSuiteAttribute
    {
        [Test]
        public void Test()
        {
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("BaseSuite"));
        }
    }

    public class DerivedTestClass_WithItsOwnGroboTestSuiteAttribute_Test
    {
        [Test]
        public void Test()
        {
            var testResults = TestRunner.RunTestClass<DerivedTestClass_WithItsOwnGroboTestSuiteAttribute_ExplicitTest>();
            var result = testResults[nameof(DerivedTestClass_WithItsOwnGroboTestSuiteAttribute_ExplicitTest.Test)];
            result.Message.Should().Contain("There are multiple suite names (BaseSuite, BaseSuite) defined for:");
        }
    }
}