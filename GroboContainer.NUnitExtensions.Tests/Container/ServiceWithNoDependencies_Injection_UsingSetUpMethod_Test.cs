using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [GroboTestFixture]
    public class ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test : GroboTestMachineryTestBase
    {
        [GroboSetUp]
        public void SetUp()
        {
            GroboTestMachineryTrace.Log($"SetUp() for {GroboTestContext.Current.SuiteName()}");
            var serviceFromContainer = GroboTestContext.Current.Container.Get<IServiceWithNoDependencies>();
            if (serviceWithNoDependencies == null)
                serviceWithNoDependencies = serviceFromContainer;
            else
                Assert.That(serviceFromContainer, Is.SameAs(serviceWithNoDependencies));
        }

        [Test]
        public void Test01()
        {
            serviceWithNoDependencies.Foo(1);
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    $"SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "ServiceWithNoDependencies.Foo(p=1)",
                });
        }

        [Test]
        public void Test02()
        {
            serviceWithNoDependencies.Foo(2);
            AssertTestMachineryTrace(new[]
                {
                    "SuiteWrapper.SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test",
                    "MethodWrapper.SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test::GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test.Test01",
                    "SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test",
                    "ServiceWithNoDependencies.Foo(p=1)",
                    "MethodWrapper.TearDown() for GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test::GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test.Test01",
                    "MethodWrapper.SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test::GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test.Test02",
                    "SetUp() for GroboContainer.NUnitExtensions.Tests.Container.ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test",
                    "ServiceWithNoDependencies.Foo(p=2)",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }

        private IServiceWithNoDependencies serviceWithNoDependencies;
    }
}