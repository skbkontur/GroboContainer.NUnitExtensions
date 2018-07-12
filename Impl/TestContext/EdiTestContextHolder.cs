using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public static class EdiTestContextHolder
    {
        [NotNull]
        public static EdiTestContext GetCurrentContext()
        {
            if (currentMethodContext == null)
                throw new InvalidProgramStateException("Current test context is not set");
            return new EdiTestContext(currentTestName, currentSuiteContext, currentMethodContext);
        }

        public static void SetCurrentContext([NotNull] string testName, [NotNull] EdiTestSuiteContextData suiteContext, [NotNull] EdiTestMethodContextData methodContext)
        {
            currentTestName = testName;
            currentSuiteContext = suiteContext;
            currentMethodContext = methodContext;
        }

        [NotNull]
        public static EdiTestMethodContextData ResetCurrentTestContext()
        {
            var methodContext = currentMethodContext;
            if (methodContext == null)
                throw new InvalidProgramStateException("Current test context is not set");
            currentMethodContext = null;
            currentSuiteContext = null;
            currentTestName = null;
            return methodContext;
        }

        private static string currentTestName;
        private static EdiTestSuiteContextData currentSuiteContext;
        private static EdiTestMethodContextData currentMethodContext;
    }
}