using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionFailures
{
    [GroboTestSuite("TestFixtureSetUpMethod_InGroboTestSuite_Test")]
    [Explicit("Intentionally fails with 'GroboTestFixtureSetUp method is only allowed inside GroboTestFixture suite' error")]
    public class TestFixtureSetUpMethod_InGroboTestSuite_ExplicitTest : GroboTestMachineryTestBase
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
        }
    }

    public class TestFixtureSetUpMethod_InGroboTestSuite_Test
    {
        [Test]
        public void Test()
        {
            var testResults = TestRunner.RunTestClass<TestFixtureSetUpMethod_InGroboTestSuite_ExplicitTest>();
            var result = testResults[nameof(TestFixtureSetUpMethod_InGroboTestSuite_ExplicitTest.Test)];
            result.Message.Should().Contain("GroboTestFixtureSetUp method is only allowed inside GroboTestFixture suite.");
        }
    }
}