using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [AndU("11")]
    public class AndV : EdiTestMethodWrapperAttribute
    {
        public AndV(string t)
        {
            this.t = t;
        }

        public override void SetUp(string suiteName, IEdiTestContextData suiteContext, IEdiTestContextData methodContext)
        {
            EdiTestMachineryTrace.Log(string.Format("AndV(t={0}).SetUp()", t), methodContext);
        }

        public override void TearDown(string suiteName, IEdiTestContextData suiteContext, IEdiTestContextData methodContext)
        {
            EdiTestMachineryTrace.Log(string.Format("AndV(t={0}).TearDown()", t), methodContext);
        }

        protected override string TryGetIdentity()
        {
            return t;
        }

        private readonly string t;
    }
}