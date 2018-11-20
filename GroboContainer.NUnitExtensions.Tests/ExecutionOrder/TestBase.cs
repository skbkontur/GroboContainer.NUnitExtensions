using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [EdiTestSuite("InheritanceHierarchy")]
    public class TestBase : EdiTestMachineryTestBase
    {
        [Test]
        public void Test01()
        {
            EdiTestMachineryTrace.Log("Test01()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchy"));
        }
    }
}