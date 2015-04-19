using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [EdiTestSuite("BaseSuite")]
    public class DerivedTestClass_WithItsOwnEdiTestSuiteAttribute_Test : TestBaseWithEdiTestSuiteAttribute
    {
        [Test]
        [Ignore("todo [edi-test]: Intentionally fails with 'There are multiple suite names (BaseSuite, BaseSuite) defined for ...' error")]
        public void Test()
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("BaseSuite"));
        }
    }
}