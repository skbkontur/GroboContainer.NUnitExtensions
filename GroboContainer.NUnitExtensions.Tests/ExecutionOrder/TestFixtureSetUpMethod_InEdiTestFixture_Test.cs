using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [EdiTestFixture]
    public class TestFixtureSetUpMethod_InEdiTestFixture_Test : EdiTestMachineryTestBase
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log("TestFixtureSetUp()");
        }

        [Test]
        public void Test()
        {
            EdiTestMachineryTrace.Log("Test()");
            AssertEdiTestMachineryTrace(new[]
                {
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    "TestFixtureSetUp()",
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "Test()",
                });
        }
    }
}