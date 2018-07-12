using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [EdiTestSuite("WithDuplicateWrappersSuite"), WithX("0"), WithX("1"), WithZ("3")]
    public class WrapperDuplicates_Test : EdiTestMachineryTestBase
    {
        [Test, AndU("10"), AndU("11"), AndV("12")]
        public void Test01()
        {
            EdiTestMachineryTrace.Log("Test01()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("WithDuplicateWrappersSuite"));
            Assert.That(EdiTestMachineryTrace.TraceLines, Is.EquivalentTo(new[]
                {
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    "WithX(p=0).SetUp()",
                    "WithX(p=1).SetUp()",
                    "WithY(q=2).SetUp()",
                    "WithZ(r=3).SetUp()",
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "AndU(s=10).SetUp()",
                    "AndU(s=11).SetUp()",
                    "AndV(t=12).SetUp()",
                    "Test01()",
                }));
        }
    }
}