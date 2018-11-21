using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [AndU("11")]
    public class AndV : GroboTestMethodWrapperAttribute
    {
        public AndV(string t)
        {
            this.t = t;
        }

        public override void SetUp(string suiteName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            GroboTestMachineryTrace.Log($"AndV(t={t}).SetUp()", methodContext);
        }

        public override void TearDown(string suiteName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            GroboTestMachineryTrace.Log($"AndV(t={t}).TearDown()", methodContext);
        }

        protected override string TryGetIdentity()
        {
            return t;
        }

        private readonly string t;
    }
}