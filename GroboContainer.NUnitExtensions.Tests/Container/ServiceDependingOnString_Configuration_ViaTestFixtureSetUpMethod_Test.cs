using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [GroboTestFixture]
    public class ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test : GroboTestMachineryTestBase
    {
        [GroboTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log($"TestFixtureSetUp() for {suiteContext.GetContextItem<string>("TestSuiteName")}");
            suiteContext.Container.Configurator.ForAbstraction<IServiceDependingOnString>().UseInstances(new ServiceDependingOnString("2"));
        }

        [Test]
        public void Test01()
        {
            serviceDependingOnString.Hoo(1);
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    $"TestFixtureSetUp() for {GroboTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "ServiceDependingOnString.Hoo(p=2, q=1)",
                });
        }

        [Test]
        public void Test02()
        {
            serviceDependingOnString.Hoo(2);
            AssertTestMachineryTrace(new[]
                {
                    "SuiteWrapper.SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test",
                    "TestFixtureSetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test",
                    "MethodWrapper.SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test::GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test.Test01",
                    "ServiceDependingOnString.Hoo(p=2, q=1)",
                    "MethodWrapper.TearDown() for GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test::GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test.Test01",
                    "MethodWrapper.SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test::GroboContainer.NUnitExtensions.Tests.Container.ServiceDependingOnString_Configuration_ViaTestFixtureSetUpMethod_Test.Test02",
                    "ServiceDependingOnString.Hoo(p=2, q=2)",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceDependingOnString serviceDependingOnString;
#pragma warning restore 649
    }
}