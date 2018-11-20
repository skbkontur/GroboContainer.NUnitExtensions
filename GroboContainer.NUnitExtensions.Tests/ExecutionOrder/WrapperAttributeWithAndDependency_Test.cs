using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
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
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    "WithA(p=0).SetUp()",
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "AndU(s=0).SetUp()",
                    "Test01()",
                }));
        }
    }
}