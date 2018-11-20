using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests
{
    [WithDebugLogPerSuite]
    public abstract class EdiTestMachineryTestBase : IDebugLogPerMethodMixin
    {
        protected EdiTestMachineryTestBase()
        {
            EdiTestMachineryTrace.ClearTrace();
        }

        protected static void AssertEdiTestMachineryTrace(string[] expectedMessages)
        {
            Assert.That(EdiTestMachineryTrace.TraceLines, Is.EqualTo(expectedMessages));
        }
    }
}