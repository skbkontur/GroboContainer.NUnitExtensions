using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    public class DerivedTestClass_Test : TestBase
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
        public void TestX()
        {
            EdiTestMachineryTrace.Log("TestX()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchy"));
            AssertEdiTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.DerivedTestClass_Test.Test01",
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    $"MethodWrapper.TearDown() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.DerivedTestClass_Test.Test01",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.DerivedTestClass_Test.TestX",
                    "SetUp()",
                    "TestX()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}