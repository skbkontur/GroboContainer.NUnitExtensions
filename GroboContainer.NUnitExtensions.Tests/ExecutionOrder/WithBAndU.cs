using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [WithDebugLogPerSuite]
    [AndU("0")]
    public class WithBAndU : GroboTestSuiteWrapperAttribute
    {
        public WithBAndU(string p)
        {
            this.p = p;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log($"WithA(p={p}).SetUp()", suiteContext);
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log($"WithA(p={p}).TearDown()", suiteContext);
        }

        protected override string TryGetIdentity()
        {
            return p;
        }

        private readonly string p;
    }
}