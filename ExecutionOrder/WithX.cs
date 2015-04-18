using System.Reflection;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [WithDebugLogPerSuite]
    public class WithX : EdiTestSuiteWrapperAttribute
    {
        public WithX(string p)
        {
            this.p = p;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEdiTestContextData suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithX(p={0}).SetUp()", p), suiteContext);
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEdiTestContextData suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithX(p={0}).TearDown()", p), suiteContext);
        }

        protected override string TryGetIdentity()
        {
            return p;
        }

        private readonly string p;
    }
}