using GroboContainer.Core;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [EdiTestSuite("TestFixtureSetUpMethod_InEdiTestSuite_Test")]
    public class TestFixtureSetUpMethod_InEdiTestSuite_Test : EdiTestMachineryTestBase
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp([NotNull] IContainer container)
        {
            EdiTestMachineryTrace.Log("TestFixtureSetUp()");
        }

        [Test]
        [Ignore("Intentionally fails with 'EdiTestFixtureSetUp method is only allowed inside EdiTestFixture suite' error")]
        public void Test()
        {
            EdiTestMachineryTrace.Log("Test()");
        }
    }
}