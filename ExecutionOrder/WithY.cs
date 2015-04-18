using System.Reflection;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [WithX("1")]
    public class WithY : EdiTestSuiteWrapperAttribute
    {
        public WithY(string q)
        {
            this.q = q;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEdiTestContextData suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithY(q={0}).SetUp()", q), suiteContext);
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEdiTestContextData suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithY(q={0}).TearDown()", q), suiteContext);
        }

        protected override string TryGetIdentity()
        {
            return q;
        }

        private readonly string q;
    }
}