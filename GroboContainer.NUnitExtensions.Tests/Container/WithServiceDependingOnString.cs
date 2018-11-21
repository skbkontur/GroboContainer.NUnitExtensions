using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [WithDebugLogPerSuite]
    public class WithServiceDependingOnString : GroboTestSuiteWrapperAttribute
    {
        public WithServiceDependingOnString(string p)
        {
            this.p = p;
        }

        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log($"WithServiceDependingOnString(p={p}).SetUp()", suiteContext);
            suiteContext.Container.Configurator.ForAbstraction<IServiceDependingOnString>().UseInstances(new ServiceDependingOnString(p));
        }

        protected override string TryGetIdentity()
        {
            return p;
        }

        private readonly string p;
    }
}