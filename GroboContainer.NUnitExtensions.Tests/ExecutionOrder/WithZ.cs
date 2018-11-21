using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [WithY("2")]
    public class WithZ : GroboTestSuiteWrapperAttribute
    {
        public WithZ(string r)
        {
            this.r = r;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log($"WithZ(r={r}).SetUp()", suiteContext);
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log($"WithZ(r={r}).TearDown()", suiteContext);
        }

        protected override string TryGetIdentity()
        {
            return r;
        }

        private readonly string r;
    }
}