using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    public abstract class TestBaseWithSetUpMethod : EdiTestMachineryTestBase
    {
        [EdiSetUp]
        public void TestBase_SetUp()
        {
            EdiTestMachineryTrace.Log("TestBase_SetUp()");
        }

        [EdiTearDown]
        public void TestBase_TearDown()
        {
            EdiTestMachineryTrace.Log("TestBase_TearDown()");
        }
    }
}