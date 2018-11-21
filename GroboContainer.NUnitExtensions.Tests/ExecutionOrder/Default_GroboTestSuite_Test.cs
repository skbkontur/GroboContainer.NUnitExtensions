using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestSuite]
    public class Default_GroboTestSuite_Test : GroboTestMachineryTestBase
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
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("DefaultSuite"));
            Assert.That(GroboTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test01"));
            GroboTestMachineryTrace.Log("Test01()");
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "SetUp()",
                    "Test01()",
                });
        }

        [Test]
        public void Test02()
        {
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("DefaultSuite"));
            Assert.That(GroboTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test02"));
            GroboTestMachineryTrace.Log("Test02()");
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.Default_GroboTestSuite_Test.Test01",
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    $"MethodWrapper.TearDown() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.Default_GroboTestSuite_Test.Test01",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.Default_GroboTestSuite_Test.Test02",
                    "SetUp()",
                    "Test02()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}