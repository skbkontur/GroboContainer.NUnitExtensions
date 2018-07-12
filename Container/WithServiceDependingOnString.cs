using System.Reflection;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container
{
    [WithDebugLogPerSuite]
    public class WithServiceDependingOnString : EdiTestSuiteWrapperAttribute
    {
        public WithServiceDependingOnString(string p)
        {
            this.p = p;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithServiceDependingOnString(p={0}).SetUp()", p), suiteContext);
            suiteContext.Container.Configurator.ForAbstraction<IServiceDependingOnString>().UseInstances(new ServiceDependingOnString(p));
        }

        protected override string TryGetIdentity()
        {
            return p;
        }

        private readonly string p;
    }
}