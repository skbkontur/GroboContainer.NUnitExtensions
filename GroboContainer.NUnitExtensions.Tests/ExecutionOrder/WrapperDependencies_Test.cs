using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestSuite("WithWrappersSuite"), WithZ("3"), AndV("12")]
    public class WrapperDependencies_Test : GroboTestMachineryTestBase
    {
        [GroboSetUp]
        public void SetUp()
        {
            GroboTestMachineryTrace.Log("SetUp()");
        }

        [GroboTearDown]
        public void TearDown()
        {
            GroboTestMachineryTrace.Log("TearDown()");
        }

        [Test]
        public void Test01()
        {
            GroboTestMachineryTrace.Log("Test01()");
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("WithWrappersSuite"));
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "WithX(p=1).SetUp()",
                    "WithY(q=2).SetUp()",
                    "WithZ(r=3).SetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "SetUp()",
                    "Test01()",
                });
        }

        [Test]
        public void Test02()
        {
            GroboTestMachineryTrace.Log("Test02()");
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("WithWrappersSuite"));
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "WithX(p=1).SetUp()",
                    "WithY(q=2).SetUp()",
                    "WithZ(r=3).SetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.WrapperDependencies_Test.Test01",
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    "AndV(t=12).TearDown()",
                    "AndU(s=11).TearDown()",
                    $"MethodWrapper.TearDown() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.WrapperDependencies_Test.Test01",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.WrapperDependencies_Test.Test02",
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "SetUp()",
                    "Test02()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}