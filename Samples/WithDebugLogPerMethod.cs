using System;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Samples
{
    public class WithDebugLogPerMethod : EdiTestMethodWrapperAttribute
    {
        public override sealed void SetUp([NotNull] string testName, [NotNull] IEdiTestContextData suiteContext, [NotNull] IEdiTestContextData methodContext)
        {
            methodContext.AddItem("MethodDebugId", Guid.NewGuid());
            methodContext.AddItem("TestName", testName);
            Console.Out.WriteLine("MethodWrapper.SetUp() for {0}::{1}; MethodContext: {2}", suiteContext.GetItem("TestSuiteName"), testName, methodContext);
        }

        public override sealed void TearDown([NotNull] string testName, [NotNull] IEdiTestContextData suiteContext, [NotNull] IEdiTestContextData methodContext)
        {
            Console.Out.WriteLine("MethodWrapper.TearDown() for {0}::{1}; MethodContext: {2}", suiteContext.GetItem("TestSuiteName"), testName, methodContext);
            Assert.That(testName, Is.EqualTo(methodContext.GetItem("TestName")));
            Assert.That(methodContext.RemoveItem("TestName"), Is.True);
        }
    }
}