using System;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery
{
    public class AndDebugLogPerMethod : EdiTestMethodWrapperAttribute
    {
        public override sealed void SetUp([NotNull] string testName, [NotNull] IEdiTestContextData suiteContext, [NotNull] IEdiTestContextData methodContext)
        {
            Assert.That(currentMethodDebugId, Is.Null);
            currentMethodDebugId = Guid.NewGuid();
            methodContext.AddItem("MethodDebugId", currentMethodDebugId);
            methodContext.AddItem("TestName", testName);
            EdiTestMachineryTrace.Log(string.Format("MethodWrapper.SetUp() for {0}::{1}", suiteContext.GetItem("TestSuiteName"), testName), methodContext);
        }

        public override sealed void TearDown([NotNull] string testName, [NotNull] IEdiTestContextData suiteContext, [NotNull] IEdiTestContextData methodContext)
        {
            EdiTestMachineryTrace.Log(string.Format("MethodWrapper.TearDown() for {0}::{1}", suiteContext.GetItem("TestSuiteName"), testName), methodContext);
            Assert.That(testName, Is.EqualTo(methodContext.GetItem("TestName")));
            Assert.That(methodContext.RemoveItem("TestName"), Is.True);
            Assert.That(methodContext.GetItem("MethodDebugId"), Is.EqualTo(currentMethodDebugId));
            Assert.That(methodContext.RemoveItem("MethodDebugId"), Is.True);
            currentMethodDebugId = null;
        }

        private static Guid? currentMethodDebugId;
    }
}