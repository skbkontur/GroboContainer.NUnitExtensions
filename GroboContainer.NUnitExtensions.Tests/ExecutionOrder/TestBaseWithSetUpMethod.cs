namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    public abstract class TestBaseWithSetUpMethod : GroboTestMachineryTestBase
    {
        [GroboSetUp]
        public void TestBase_SetUp()
        {
            GroboTestMachineryTrace.Log("TestBase_SetUp()");
        }

        [GroboTearDown]
        public void TestBase_TearDown()
        {
            GroboTestMachineryTrace.Log("TestBase_TearDown()");
        }
    }
}