using System;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Samples
{
    public class WithDebugLogPerMethod : EdiTestMethodWrapperAttribute
    {
        public override sealed void SetUp([NotNull] string testName, [NotNull] EdiTestSuiteContextData suiteContext, [NotNull] EdiTestMethodContextData methodContext)
        {
            methodContext.Items.Add("MethodDebugId", Guid.NewGuid());
            methodContext.Items.Add("TestName", testName);
            Console.Out.WriteLine("MethodWrapper.SetUp() for {0}::{1}; MethodContext: {2}", suiteContext.Items["TestSuiteName"], testName, methodContext);
        }

        public override sealed void TearDown([NotNull] string testName, [NotNull] EdiTestSuiteContextData suiteContext, [NotNull] EdiTestMethodContextData methodContext)
        {
            Console.Out.WriteLine("MethodWrapper.TearDown() for {0}::{1}; MethodContext: {2}", suiteContext.Items["TestSuiteName"], testName, methodContext);
            Assert.That(testName, Is.EqualTo(methodContext.Items["TestName"]));
            Assert.That(methodContext.Items.Remove("TestName"), Is.True);
        }
    }
}