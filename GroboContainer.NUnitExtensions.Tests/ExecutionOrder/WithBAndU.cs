using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [WithDebugLogPerSuite]
    [AndU("0")]
    public class WithBAndU : EdiTestSuiteWrapperAttribute
    {
        public WithBAndU(string p)
        {
            this.p = p;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithA(p={0}).SetUp()", p), suiteContext);
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithA(p={0}).TearDown()", p), suiteContext);
        }

        protected override string TryGetIdentity()
        {
            return p;
        }

        private readonly string p;
    }
}