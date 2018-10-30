using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionFailures
{
    [EdiTestSuite("TestFixtureSetUpMethod_InEdiTestSuite_Test")]
    [Explicit("Intentionally fails with 'EdiTestFixtureSetUp method is only allowed inside EdiTestFixture suite' error")]
    public class TestFixtureSetUpMethod_InEdiTestSuite_ExplicitTest : EdiTestMachineryTestBase
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
        }
    }

    public class TestFixtureSetUpMethod_InEdiTestSuite_Test
    {
        [Test]
        public void Test()
        {
            var testResults = TestRunner.RunTestClass<TestFixtureSetUpMethod_InEdiTestSuite_ExplicitTest>();
            var result = testResults[nameof(TestFixtureSetUpMethod_InEdiTestSuite_ExplicitTest.Test)];
            result.Message.Should().Contain("EdiTestFixtureSetUp method is only allowed inside EdiTestFixture suite.");
        }
    }
}