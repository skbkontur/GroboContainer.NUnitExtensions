using GroboContainer.Core;
using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
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
            return $"TestName: {testName}, TestSuiteContext: {suiteContext}, TestMethodContext: {methodContext}";
        }

        private readonly string testName;
        private readonly IEditableEdiTestContext suiteContext;
        private readonly IEditableEdiTestContext methodContext;
    }
}