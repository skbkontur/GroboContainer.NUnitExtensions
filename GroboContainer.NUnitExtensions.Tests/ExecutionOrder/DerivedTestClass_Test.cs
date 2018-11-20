using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
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
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.DerivedTestClass_Test.Test01", EdiTestContext.Current.SuiteName()),
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    string.Format("MethodWrapper.TearDown() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.DerivedTestClass_Test.Test01", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.DerivedTestClass_Test.TestX", EdiTestContext.Current.SuiteName()),
                    "SetUp()",
                    "TestX()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}