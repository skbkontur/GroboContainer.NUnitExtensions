using FluentAssertions;

using GroboContainer.NUnitExtensions.Tests.ExecutionOrder;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionFailures
{
    [EdiTestSuite("InheritanceHierarchyForSetUpMethod2")]
    [Explicit("Intentionally fails with 'There are multiple methods marked with EdiSetUp/EdiTearDown attribute in ...' error")]
    public class DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest : TestBaseWithSetUpMethod
    {
        [EdiSetUp]
        public void SetUp()
        {
            EdiTestMachineryTrace.Log("SetUp()");
        }

        [EdiTearDown]
        public void TearDown()
        {
            EdiTestMachineryTrace.Log("TearDown()");
        }

        [Test]
        public void Test01()
        {
            EdiTestMachineryTrace.Log("Test01()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchyForSetUpMethod"));
        }
    }

    public class DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_Test
    {
        [Test]
        public void Test01()
        {
            var testResults = TestRunner.RunTestClass<DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest>();
            var result = testResults[nameof(DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest.Test01)];
            result.Message.Should().Contain("There are multiple methods marked with EdiSetUpAttribute attribute in:");
        }
    }
}