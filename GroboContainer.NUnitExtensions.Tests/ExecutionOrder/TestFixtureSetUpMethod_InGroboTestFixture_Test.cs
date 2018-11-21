using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestFixture]
    public class TestFixtureSetUpMethod_InGroboTestFixture_Test : GroboTestMachineryTestBase
    {
        [GroboTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log("TestFixtureSetUp()");
        }

        [Test]
        public void Test()
        {
            GroboTestMachineryTrace.Log("Test()");
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "TestFixtureSetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "Test()",
                });
        }
    }
}