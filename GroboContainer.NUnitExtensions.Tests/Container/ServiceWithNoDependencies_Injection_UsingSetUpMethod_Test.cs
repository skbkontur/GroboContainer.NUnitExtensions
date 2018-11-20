using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [EdiTestFixture]
    public class ServiceWithNoDependencies_Injection_UsingSetUpMethod_Test : EdiTestMachineryTestBase
    {
        [EdiSetUp]
        public void SetUp()
        {
            EdiTestMachineryTrace.Log(string.Format("SetUp() for {0}", EdiTestContext.Current.SuiteName()));
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
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    string.Format("SetUp() for {0}", EdiTestContext.Current.SuiteName()),
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