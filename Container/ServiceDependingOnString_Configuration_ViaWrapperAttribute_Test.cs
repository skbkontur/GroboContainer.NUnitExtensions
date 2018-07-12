using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container
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
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    "WithServiceDependingOnString(p=1).SetUp()",
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "ServiceDependingOnString.Hoo(p=1, q=0)",
                });
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceDependingOnString serviceDependingOnString;
#pragma warning restore 649
    }
}