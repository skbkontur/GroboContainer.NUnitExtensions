using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestSuite("WithDuplicateWrappersSuite"), WithX("0"), WithX("1"), WithZ("3")]
    public class WrapperDuplicates_Test : GroboTestMachineryTestBase
    {
        [Test, AndU("10"), AndU("11"), AndV("12")]
        public void Test01()
        {
            GroboTestMachineryTrace.Log("Test01()");
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("WithDuplicateWrappersSuite"));
            Assert.That(GroboTestMachineryTrace.TraceLines, Is.EquivalentTo(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "WithX(p=0).SetUp()",
                    "WithX(p=1).SetUp()",
                    "WithY(q=2).SetUp()",
                    "WithZ(r=3).SetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "AndU(s=10).SetUp()",
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "Test01()",
                }));
        }
    }
}