using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [EdiTestSuite("TestFixtureSetUpMethod_InEdiTestSuite_Test")]
    public class TestFixtureSetUpMethod_InEdiTestSuite_Test : EdiTestMachineryTestBase
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log("TestFixtureSetUp()");
        }

        [Test]
        [Ignore("Intentionally fails with 'EdiTestFixtureSetUp method is only allowed inside EdiTestFixture suite' error")]
        public void Test()
        {
            EdiTestMachineryTrace.Log("Test()");
        }
    }
}