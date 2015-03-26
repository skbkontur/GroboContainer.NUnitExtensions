using System.Reflection;

using JetBrains.Annotations;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public abstract class EdiTestSuiteWrapperAttribute : EdiTestWrapperAttribute
    {
        public virtual void SetUp([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEdiTestContextData suiteContext)
        {
        }

        public virtual void TearDown([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEdiTestContextData suiteContext)
        {
        }
    }
}