using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [AndDebugLogPerMethod]
    public class AndU : GroboTestMethodWrapperAttribute
    {
        public AndU(string s)
        {
            this.s = s;
        }

        public override void SetUp(string testName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            GroboTestMachineryTrace.Log($"AndU(s={s}).SetUp()", methodContext);
        }

        public override void TearDown(string testName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            GroboTestMachineryTrace.Log($"AndU(s={s}).TearDown()", methodContext);
        }

        protected override string TryGetIdentity()
        {
            return s;
        }

        private readonly string s;
    }
}