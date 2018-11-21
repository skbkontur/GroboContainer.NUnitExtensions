using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests
{
    [WithDebugLogPerSuite]
    public abstract class GroboTestMachineryTestBase : IDebugLogPerMethodMixin
    {
        protected GroboTestMachineryTestBase()
        {
            GroboTestMachineryTrace.ClearTrace();
        }

        protected static void AssertTestMachineryTrace(string[] expectedMessages)
        {
            Assert.That(GroboTestMachineryTrace.TraceLines, Is.EqualTo(expectedMessages));
        }
    }
}