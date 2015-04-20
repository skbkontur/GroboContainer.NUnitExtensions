using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [AndDebugLogPerMethod]
    public class AndU : EdiTestMethodWrapperAttribute
    {
        public AndU(string s)
        {
            this.s = s;
        }

        public override void SetUp(string suiteName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log(string.Format("AndU(s={0}).SetUp()", s), methodContext);
        }

        public override void TearDown(string suiteName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log(string.Format("AndU(s={0}).TearDown()", s), methodContext);
        }

        protected override string TryGetIdentity()
        {
            return s;
        }

        private readonly string s;
    }
}