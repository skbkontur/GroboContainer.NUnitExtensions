using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [EdiTestSuite("InheritanceHierarchyForSetUpMethod")]
    public class DerivedTestClass_WithSetUpMethodInBase_Test : TestBaseWithSetUpMethod
    {
        [Test]
        public void Test()
        {
            EdiTestMachineryTrace.Log("Test()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchyForSetUpMethod"));
            AssertEdiTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::{EdiTestContext.Current.TestName()}",
                    "TestBase_SetUp()",
                    "Test()",
                });
        }
    }
}