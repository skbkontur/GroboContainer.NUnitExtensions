using JetBrains.Annotations;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public abstract class EdiTestMethodWrapperAttribute : EdiTestWrapperAttribute
    {
        public virtual void SetUp([NotNull] string testName, [NotNull] IEdiTestContextData suiteContext, [NotNull] IEdiTestContextData methodContext)
        {
        }

        public virtual void TearDown([NotNull] string testName, [NotNull] IEdiTestContextData suiteContext, [NotNull] IEdiTestContextData methodContext)
        {
        }
    }
}