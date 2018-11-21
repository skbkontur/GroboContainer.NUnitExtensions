using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    public class DerivedTestClass_Test : TestBase
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
        public void TestX()
        {
            GroboTestMachineryTrace.Log("TestX()");
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchy"));
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.DerivedTestClass_Test.Test01",
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    $"MethodWrapper.TearDown() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.DerivedTestClass_Test.Test01",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.DerivedTestClass_Test.TestX",
                    "SetUp()",
                    "TestX()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}