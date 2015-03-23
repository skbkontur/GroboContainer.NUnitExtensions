using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public static class EdiTestContextHolder
    {
        [NotNull]
        public static EdiTestContext GetCurrentTestContext()
        {
            if(currentTestMethodContext == null)
                throw new InvalidProgramStateException("Current test context is not set");
            return new EdiTestContext(currentTestName, currentTestSuiteContext, currentTestMethodContext);
        }

        public static void SetCurrentTestContext([NotNull] string testName, [NotNull] EdiTestSuiteContextData testSuiteContext, [NotNull] EdiTestMethodContextData testMethodContext)
        {
            currentTestName = testName;
            currentTestSuiteContext = testSuiteContext;
            currentTestMethodContext = testMethodContext;
        }

        [NotNull]
        public static EdiTestMethodContextData ResetCurrentTestContext()
        {
            var testMethodContext = currentTestMethodContext;
            if(testMethodContext == null)
                throw new InvalidProgramStateException("Current test context is not set");
            currentTestMethodContext = null;
            currentTestSuiteContext = null;
            currentTestName = null;
            return testMethodContext;
        }

        private static string currentTestName;
        private static EdiTestSuiteContextData currentTestSuiteContext;
        private static EdiTestMethodContextData currentTestMethodContext;
    }
}