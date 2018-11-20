using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [EdiTestSuite("ZZZZ"), WithBAndU("0")]
    public class WrapperAttributeWithAndDependency_Test : EdiTestMachineryTestBase
    {
        [Test]
        public void Test01()
        {
            EdiTestMachineryTrace.Log("Test01()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("ZZZZ"));
            Assert.That(EdiTestMachineryTrace.TraceLines, Is.EquivalentTo(new[]
                {
                    $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                    "WithA(p=0).SetUp()",
                    $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::{EdiTestContext.Current.TestName()}",
                    "AndU(s=0).SetUp()",
                    "Test01()",
                }));
        }
    }
}