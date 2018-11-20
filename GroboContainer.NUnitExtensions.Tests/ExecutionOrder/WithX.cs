using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [WithDebugLogPerSuite]
    public class WithX : EdiTestSuiteWrapperAttribute
    {
        public WithX(string p)
        {
            this.p = p;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("WithX(p={0}).SetUp()", p), suiteContext);
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
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