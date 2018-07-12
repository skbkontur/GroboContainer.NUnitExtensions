using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [EdiTestSuite("InheritanceHierarchyForSetUpMethod")]
    public class DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_Test : TestBaseWithSetUpMethod
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
        [Ignore("Intentionally fails with 'There are multiple methods marked with EdiSetUp/EdiTearDown attribute in ...' error")]
        public void Test01()
        {
            EdiTestMachineryTrace.Log("Test01()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchyForSetUpMethod"));
        }
    }
}