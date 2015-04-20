using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container
{
    [EdiTestFixture]
    public class ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test : EdiTestMachineryTestBase
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("TestFixtureSetUp() for {0}", suiteContext.GetContextItem<string>("TestSuiteName")));
            suiteContext.Container.Configurator.ForAbstraction<IServiceDependingOnString>().UseInstances(new ServiceDependingOnString("2"));
        }

        [Test]
        public void Test01()
        {
            serviceDependingOnString.Hoo(1);
            AssertEdiTestMachineryTrace(new[]
                {
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("TestFixtureSetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "ServiceDependingOnString.Hoo(p=2, q=1)",
                });
        }

        [Test]
        public void Test02()
        {
            serviceDependingOnString.Hoo(2);
            AssertEdiTestMachineryTrace(new[]
                {
                    "SuiteWrapper.SetUp() for SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test",
                    "TestFixtureSetUp() for SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test",
                    "MethodWrapper.SetUp() for SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test.Test01",
                    "ServiceDependingOnString.Hoo(p=2, q=1)",
                    "MethodWrapper.TearDown() for SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test.Test01",
                    "MethodWrapper.SetUp() for SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test.Test02",
                    "ServiceDependingOnString.Hoo(p=2, q=2)",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceDependingOnString serviceDependingOnString;
#pragma warning restore 649
    }
}