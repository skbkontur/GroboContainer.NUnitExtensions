using GroboContainer.Core;

using JetBrains.Annotations;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public class EdiTestContext : IEdiTestContext
    {
        public EdiTestContext([NotNull] string testName, [NotNull] IEditableEdiTestContext suiteContext, [NotNull] IEditableEdiTestContext methodContext)
        {
            this.testName = testName;
            this.suiteContext = suiteContext;
            this.methodContext = methodContext;
        }

        [NotNull]
        public static IEdiTestContext Current { get { return EdiTestContextHolder.GetCurrentContext(); } }

        [NotNull]
        public IContainer Container { get { return suiteContext.Container; } }

        public bool TryGetContextItem([NotNull] string itemName, out object itemValue)
        {
            if (methodContext.TryGetContextItem(itemName, out itemValue))
                return true;
            if (suiteContext.TryGetContextItem(itemName, out itemValue))
                return true;
            return false;
        }

        public override string ToString()
        {
            return string.Format("TestName: {0}, TestSuiteContext: {1}, TestMethodContext: {2}", testName, suiteContext, methodContext);
        }

        private readonly string testName;
        private readonly IEditableEdiTestContext suiteContext;
        private readonly IEditableEdiTestContext methodContext;
    }
}