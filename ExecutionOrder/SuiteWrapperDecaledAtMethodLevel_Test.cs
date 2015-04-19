using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [EdiTestSuite("WithWrappersSuite")]
    public class SuiteWrapperDecaledAtMethodLevel_Test : EdiTestMachineryTestBase
    {
        [Test, WithX("0"), AndU("0")]
        [Ignore("Intentionally fails with 'Suite wrappers (WithX) cannot be declared at method level ...' error")]
        public void Test()
        {
            EdiTestMachineryTrace.Log("Test()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("WithWrappersSuite"));
            AssertEdiTestMachineryTrace(new[]
                {
                    string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                    "WithX(p=0).SetUp()",
                    string.Format("MethodWrapper.SetUp() for {0}::{1}", EdiTestContext.Current.SuiteName(), EdiTestContext.Current.TestName()),
                    "AndU(s=0).SetUp()",
                    "Test()",
                });
        }
    }
}