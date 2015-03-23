using System;
using System.Reflection;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Samples
{
    public class WithDebugLogPerSuite : EdiTestSuiteWrapperAttribute
    {
        public override sealed void SetUp([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] EdiTestSuiteContextData suiteContext)
        {
            suiteContext.Items.Add("SuiteDebugId", Guid.NewGuid());
            suiteContext.Items.Add("TestSuiteName", suiteName);
            Console.Out.WriteLine("SuiteWrapper.SetUp() for {0}; SuiteContext: {1}", suiteName, suiteContext);
        }

        public override sealed void TearDown([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] EdiTestSuiteContextData suiteContext)
        {
            Console.Out.WriteLine("SuiteWrapper.TearDown() for {0}; SuiteContext: {1}", suiteName, suiteContext);
            Assert.That(suiteName, Is.EqualTo(suiteContext.Items["TestSuiteName"]));
            Assert.That(suiteContext.Items.Remove("TestSuiteName"), Is.True);
        }
    }
}