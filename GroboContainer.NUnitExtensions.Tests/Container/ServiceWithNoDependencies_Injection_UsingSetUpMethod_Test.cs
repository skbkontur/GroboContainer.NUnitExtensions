using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [EdiTestFixture]
    public class ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test : EdiTestMachineryTestBase
    {
        [EdiSetUp]
        public void SetUp()
        {
            EdiTestMachineryTrace.Log($"SetUp() for {EdiTestContext.Current.SuiteName()}");
            var serviceFromContainer = EdiTestContext.Current.Container.Get<IServiceWithNoDependencies>();
            if (serviceWithNoDependencies == null)
                serviceWithNoDependencies = serviceFromContainer;
            else
                Assert.That(serviceFromContainer, Is.SameAs(serviceWithNoDependencies));
        }

        [Test]
        public void Test01()
        {
            serviceWithNoDependencies.Foo(1);
            AssertEdiTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::{EdiTestContext.Current.TestName()}",
                    $"SetUp() for {EdiTestContext.Current.SuiteName()}",
                    "ServiceWithNoDependencies.Foo(p=1)",
                });
        }

        [Test]
        public void Test02()
        {
            serviceWithNoDependencies.Foo(2);
            AssertEdiTestMachineryTrace(new[]
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