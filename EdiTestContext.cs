using GroboContainer.Core;

using JetBrains.Annotations;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;
using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public class EdiTestContext : IEdiTestContext
    {
        public EdiTestContext([NotNull] string testName, [NotNull] IEdiTestContextData testSuiteContext, [NotNull] IEdiTestContextData testMethodContext)
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
                var container = TryGetContextItem<IContainer>(EdiTestContextDataExtensions.ContainerItemKey);
                if(container == null)
                    throw new InvalidProgramStateException(string.Format("Container is not set for test context: {0}", ToString()));
                return container;
            }
        }

        [CanBeNull]
        public TItem TryGetContextItem<TItem>([NotNull] string contextItemName)
        {
            object item;
            if(testMethodContext.TryGetItem(contextItemName, out item))
                return (TItem)item;
            if(testSuiteContext.TryGetItem(contextItemName, out item))
                return (TItem)item;
            return default(TItem);
        }

        public override string ToString()
        {
            return string.Format("TestName: {0}, TestSuiteContext: {1}, TestMethodContext: {2}", testName, testSuiteContext, testMethodContext);
        }

        private readonly string testName;
        private readonly IEdiTestContextData testSuiteContext;
        private readonly IEdiTestContextData testMethodContext;
    }
}