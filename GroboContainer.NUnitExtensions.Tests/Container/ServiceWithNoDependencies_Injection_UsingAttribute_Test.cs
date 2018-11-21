using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [GroboTestFixture]
    public class ServiceWithNoDependencies_Injection_UsingAttribute_Test : GroboTestMachineryTestBase
    {
        [Test]
        public void Test()
        {
            serviceWithNoDependencies.Foo(0);
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "ServiceWithNoDependencies.Foo(p=0)",
                });
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceWithNoDependencies serviceWithNoDependencies;
#pragma warning restore 649
    }
}