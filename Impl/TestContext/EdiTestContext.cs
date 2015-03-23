using GroboContainer.Core;

using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public class EdiTestContext : IEdiTestContext
    {
        public EdiTestContext([NotNull] string testName, [NotNull] EdiTestSuiteContextData testSuiteContext, [NotNull] EdiTestMethodContextData testMethodContext)
        {
            this.testName = testName;
            this.testSuiteContext = testSuiteContext;
            this.testMethodContext = testMethodContext;
        }

        [NotNull]
        public static IEdiTestContext Current { get { return EdiTestContextHolder.GetCurrentTestContext(); } }

        [NotNull]
        public IContainer Container
        {
            get
            {
                var container = GetContextItem<IContainer>(EdiTestContextData.ContainerItemKey);
                if(container == null)
                    throw new InvalidProgramStateException(string.Format("Container is not set for test context: {0}", ToString()));
                return container;
            }
        }

        [CanBeNull]
        public TItem GetContextItem<TItem>([NotNull] string contextItemName)
        {
            object item;
            if(testMethodContext.Items.TryGetValue(contextItemName, out item))
                return (TItem)item;
            if(testSuiteContext.Items.TryGetValue(contextItemName, out item))
                return (TItem)item;
            return default(TItem);
        }

        public override string ToString()
        {
            return string.Format("TestName: {0}, TestSuiteContext: {1}, TestMethodContext: {2}", testName, testSuiteContext, testMethodContext);
        }

        private readonly string testName;
        private readonly EdiTestSuiteContextData testSuiteContext;
        private readonly EdiTestMethodContextData testMethodContext;
    }
}