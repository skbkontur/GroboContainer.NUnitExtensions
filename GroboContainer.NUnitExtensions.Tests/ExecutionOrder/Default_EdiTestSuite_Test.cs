using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [EdiTestSuite]
    public class Default_EdiTestSuite_Test : EdiTestMachineryTestBase
    {
        [EdiSetUp]
        public void SetUp()
        {
            EdiTestMachineryTrace.Log("SetUp()");
        }

        [EdiTearDown]
        public void TearDown()
        {
            EdiTestMachineryTrace.Log("TearDown()");
        }

        [Test]
        public void Test01()
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("DefaultSuite"));
            Assert.That(EdiTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test01"));
            EdiTestMachineryTrace.Log("Test01()");
            AssertEdiTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::{EdiTestContext.Current.TestName()}",
                    "SetUp()",
                    "Test01()",
                });
        }

        [Test]
        public void Test02()
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("DefaultSuite"));
            Assert.That(EdiTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test02"));
            EdiTestMachineryTrace.Log("Test02()");
            AssertEdiTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.Default_EdiTestSuite_Test.Test01",
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    $"MethodWrapper.TearDown() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.Default_EdiTestSuite_Test.Test01",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.Default_EdiTestSuite_Test.Test02",
                    "SetUp()",
                    "Test02()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}