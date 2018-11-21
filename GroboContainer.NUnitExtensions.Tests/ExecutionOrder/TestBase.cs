using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestSuite("InheritanceHierarchy")]
    public class TestBase : GroboTestMachineryTestBase
    {
        [Test]
        public void Test01()
        {
            GroboTestMachineryTrace.Log("Test01()");
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchy"));
        }
    }
}