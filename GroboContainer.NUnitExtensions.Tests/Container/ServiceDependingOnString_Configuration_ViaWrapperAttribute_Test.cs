using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [GroboTestFixture, WithServiceDependingOnString("1")]
    public class ServiceDependingOnString_Configuration_ViaWrapperAttribute_Test : GroboTestMachineryTestBase
    {
        [Test]
        public void Test()
        {
            serviceDependingOnString.Hoo(0);
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "WithServiceDependingOnString(p=1).SetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "ServiceDependingOnString.Hoo(p=1, q=0)",
                });
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceDependingOnString serviceDependingOnString;
#pragma warning restore 649
    }
}