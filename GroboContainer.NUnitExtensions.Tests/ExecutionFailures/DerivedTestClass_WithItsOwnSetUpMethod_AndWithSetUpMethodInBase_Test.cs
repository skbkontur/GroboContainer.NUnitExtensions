using FluentAssertions;

using GroboContainer.NUnitExtensions.Tests.ExecutionOrder;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionFailures
{
    [GroboTestSuite("InheritanceHierarchyForSetUpMethod2")]
    [Explicit("Intentionally fails with 'There are multiple methods marked with GroboSetUp/GroboTearDown attribute in ...' error")]
    public class DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest : TestBaseWithSetUpMethod
    {
        [GroboSetUp]
        public void SetUp()
        {
            GroboTestMachineryTrace.Log("SetUp()");
        }

        [GroboTearDown]
        public void TearDown()
        {
            GroboTestMachineryTrace.Log("TearDown()");
        }

        [Test]
        public void Test01()
        {
            GroboTestMachineryTrace.Log("Test01()");
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchyForSetUpMethod"));
        }
    }

    public class DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_Test
    {
        [Test]
        public void Test01()
        {
            var testResults = TestRunner.RunTestClass<DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest>();
            var result = testResults[nameof(DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest.Test01)];
            result.Message.Should().Contain("There are multiple methods marked with GroboSetUpAttribute attribute in:");
        }
    }
}