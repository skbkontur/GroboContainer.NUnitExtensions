using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestSuite("ZZZZ"), WithBAndU("0")]
    public class WrapperAttributeWithAndDependency_Test : GroboTestMachineryTestBase
    {
        [Test]
        public void Test01()
        {
            GroboTestMachineryTrace.Log("Test01()");
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("ZZZZ"));
            Assert.That(GroboTestMachineryTrace.TraceLines, Is.EquivalentTo(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "WithA(p=0).SetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "AndU(s=0).SetUp()",
                    "Test01()",
                }));
        }
    }
}