using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
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
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "TestBase_SetUp()",
                    "Test()",
                });
        }
    }
}