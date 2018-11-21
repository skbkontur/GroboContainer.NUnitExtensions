using GroboContainer.Core;
using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    public class GroboTestContext : IGroboTestContext
    {
        public GroboTestContext([NotNull] string testName, [NotNull] IEditableGroboTestContext suiteContext, [NotNull] IEditableGroboTestContext methodContext)
        {
            this.testName = testName;
            this.suiteContext = suiteContext;
            this.methodContext = methodContext;
        }

        [NotNull]
        public static IGroboTestContext Current => GroboTestContextHolder.GetCurrentContext();

        [NotNull]
        public IContainer Container => suiteContext.Container;

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
        private readonly IEditableGroboTestContext suiteContext;
        private readonly IEditableGroboTestContext methodContext;
    }
}