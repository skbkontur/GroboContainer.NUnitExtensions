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
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    "TestFixtureSetUp()",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::{EdiTestContext.Current.TestName()}",
                    "Test()",
                });
        }
    }
}