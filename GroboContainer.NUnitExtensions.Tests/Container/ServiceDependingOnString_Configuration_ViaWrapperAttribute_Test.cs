using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [EdiTestFixture, WithServiceDependingOnString("1")]
    public class ServiceDependingOnString_Configuration_ViaWrapperAttribute_Test : EdiTestMachineryTestBase
    {
        [Test]
        public void Test()
        {
            serviceDependingOnString.Hoo(0);
            AssertEdiTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    "WithServiceDependingOnString(p=1).SetUp()",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::{EdiTestContext.Current.TestName()}",
                    "ServiceDependingOnString.Hoo(p=1, q=0)",
                });
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceDependingOnString serviceDependingOnString;
#pragma warning restore 649
    }
}