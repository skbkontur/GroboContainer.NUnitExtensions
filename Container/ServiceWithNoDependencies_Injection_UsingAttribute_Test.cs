using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.CommonWrappers;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container
{
    [EdiTestFixture, WithContainerPerSuite]
    public class ServiceWithNoDependencies_Injection_UsingAttribute_Test : EdiTestMachineryTestBase
    {
        [Test]
        public void Test()
        {
            serviceWithNoDependencies.Foo(0);
            AssertEdiTestMachineryTrace(new[]
                {
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "ServiceWithNoDependencies.Foo(p=0)",
                });
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceWithNoDependencies serviceWithNoDependencies;
#pragma warning restore 649
    }
}