using System;
using System.Reflection;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery
{
    public class WithDebugLogPerSuite : EdiTestSuiteWrapperAttribute
    {
        public override sealed void SetUp([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEdiTestContextData suiteContext)
        {
            suiteContext.AddItem("SuiteDebugId", Guid.NewGuid());
            suiteContext.AddItem("TestSuiteName", suiteName);
            EdiTestMachineryTrace.Log(string.Format("SuiteWrapper.SetUp() for {0}", suiteName), suiteContext);
        }

        public override sealed void TearDown([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEdiTestContextData suiteContext)
        {
            EdiTestMachineryTrace.Log(string.Format("SuiteWrapper.TearDown() for {0}", suiteName), suiteContext);
            Assert.That(suiteName, Is.EqualTo(suiteContext.GetItem("TestSuiteName")));
            Assert.That(suiteContext.RemoveItem("TestSuiteName"), Is.True);
        }
    }
}