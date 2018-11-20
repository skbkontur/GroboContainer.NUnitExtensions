using System;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests
{
    public class AndDebugLogPerMethod : EdiTestMethodWrapperAttribute
    {
        public override sealed void SetUp([NotNull] string testName, [NotNull] IEditableEdiTestContext suiteContext, [NotNull] IEditableEdiTestContext methodContext)
        {
            Assert.That(currentMethodDebugId, Is.Null);
            currentMethodDebugId = Guid.NewGuid();
            methodContext.AddItem("MethodDebugId", currentMethodDebugId);
            methodContext.AddItem("TestName", testName);
            EdiTestMachineryTrace.Log(string.Format("MethodWrapper.SetUp() for {0}::{1}", suiteContext.GetContextItem<string>("TestSuiteName"), testName), methodContext);
        }

        public override sealed void TearDown([NotNull] string testName, [NotNull] IEditableEdiTestContext suiteContext, [NotNull] IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log(string.Format("MethodWrapper.TearDown() for {0}::{1}", suiteContext.GetContextItem<string>("TestSuiteName"), testName), methodContext);
            Assert.That(testName, Is.EqualTo(methodContext.GetContextItem<string>("TestName")));
            Assert.That(methodContext.RemoveItem("TestName"), Is.True);
            Assert.That(methodContext.GetContextItem<Guid?>("MethodDebugId"), Is.EqualTo(currentMethodDebugId));
            Assert.That(methodContext.RemoveItem("MethodDebugId"), Is.True);
            currentMethodDebugId = null;
        }

        private static Guid? currentMethodDebugId;
    }
}