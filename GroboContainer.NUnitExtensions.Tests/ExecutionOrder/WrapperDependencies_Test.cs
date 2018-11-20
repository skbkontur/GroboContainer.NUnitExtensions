using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [EdiTestSuite("WithWrappersSuite"), WithZ("3"), AndV("12")]
    public class WrapperDependencies_Test : EdiTestMachineryTestBase
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
            EdiTestMachineryTrace.Log("Test01()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("WithWrappersSuite"));
            AssertEdiTestMachineryTrace(new[]
                {
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    "WithX(p=1).SetUp()",
                    "WithY(q=2).SetUp()",
                    "WithZ(r=3).SetUp()",
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "SetUp()",
                    "Test01()",
                });
        }

        [Test]
        public void Test02()
        {
            EdiTestMachineryTrace.Log("Test02()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("WithWrappersSuite"));
            AssertEdiTestMachineryTrace(new[]
                {
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    "WithX(p=1).SetUp()",
                    "WithY(q=2).SetUp()",
                    "WithZ(r=3).SetUp()",
                    string.Format("MethodWrapper.SetUp() for {0}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.WrapperDependencies_Test.Test01", EdiTestContext.Current.SuiteName()),
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    "AndV(t=12).TearDown()",
                    "AndU(s=11).TearDown()",
                    string.Format("MethodWrapper.TearDown() for {0}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.WrapperDependencies_Test.Test01", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.WrapperDependencies_Test.Test02", EdiTestContext.Current.SuiteName()),
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "SetUp()",
                    "Test02()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}