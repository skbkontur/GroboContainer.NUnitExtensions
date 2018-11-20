using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [AndU("11")]
    public class AndV : EdiTestMethodWrapperAttribute
    {
        public AndV(string t)
        {
            this.t = t;
        }

        public override void SetUp(string suiteName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log($"AndV(t={t}).SetUp()", methodContext);
        }

        public override void TearDown(string suiteName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log($"AndV(t={t}).TearDown()", methodContext);
        }

        protected override string TryGetIdentity()
        {
            return t;
        }

        private readonly string t;
    }
}