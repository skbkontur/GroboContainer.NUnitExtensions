using GroboContainer.Core;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.CommonWrappers;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [EdiTestFixture, WithContainerPerSuite]
    public class TestFixtureSetUpMethod_InEdiTestFixture_Test : EdiTestMachineryTestBase
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IContainer container)
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