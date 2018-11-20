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
            EdiTestMachineryTrace.Log($"WithX(p={p}).SetUp()", suiteContext);
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log($"WithX(p={p}).TearDown()", suiteContext);
        }

        protected override string TryGetIdentity()
        {
            return p;
        }

        private readonly string p;
    }
}