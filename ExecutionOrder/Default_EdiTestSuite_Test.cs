using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
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
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
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
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.Default_EdiTestSuite_Test.Test01", EdiTestContext.Current.SuiteName()),
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    string.Format("MethodWrapper.TearDown() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.Default_EdiTestSuite_Test.Test01", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.Default_EdiTestSuite_Test.Test02", EdiTestContext.Current.SuiteName()),
                    "SetUp()",
                    "Test02()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}