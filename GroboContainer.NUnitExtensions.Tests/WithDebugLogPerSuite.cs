using System;
using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests
{
    public class WithDebugLogPerSuite : GroboTestSuiteWrapperAttribute
    {
        public override sealed void SetUp([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEditableGroboTestContext suiteContext)
        {
            suiteContext.AddItem("SuiteDebugId", Guid.NewGuid());
            suiteContext.AddItem("TestSuiteName", suiteName);
            GroboTestMachineryTrace.Log($"SuiteWrapper.SetUp() for {suiteName}", suiteContext);
        }

        public override sealed void TearDown([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEditableGroboTestContext suiteContext)
        {
            GroboTestMachineryTrace.Log($"SuiteWrapper.TearDown() for {suiteName}", suiteContext);
            Assert.That(suiteName, Is.EqualTo(suiteContext.GetContextItem<string>("TestSuiteName")));
            Assert.That(suiteContext.RemoveItem("TestSuiteName"), Is.True);
        }
    }
}