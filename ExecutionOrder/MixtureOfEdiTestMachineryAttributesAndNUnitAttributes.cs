using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [TestFixture]
    public class MixtureOfEdiTestMachineryAttributesAndNUnitAttributes
    {
        [TestFixture]
        [EdiTestSuite]
        public class NUnit_TestFixtureAttribute_IsProhibited_Test
        {
            [Test]
            [Ignore("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
            public void Test()
            {
            }
        }

        [EdiTestSuite]
        public class NUnit_SetUpAttribute_IsProhibited_Test
        {
            [SetUp]
            public void SetUp()
            {
            }

            [Test]
            [Ignore("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
            public void Test()
            {
            }
        }

        [EdiTestSuite]
        public class NUnit_TearDownAttribute_IsProhibited_Test
        {
            [TearDown]
            public void TearDown()
            {
            }

            [Test]
            [Ignore("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
            public void Test()
            {
            }
        }

        [EdiTestSuite]
        public class NUnit_TestFixtureSetUpAttribute_IsProhibited_Test
        {
            [OneTimeSetUp]
            public void TestFixtureSetUp()
            {
            }

            [Test]
            [Ignore("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
            public void Test()
            {
            }
        }

        [EdiTestSuite]
        public class NUnit_TestFixtureTearDownAttribute_IsProhibited_Test
        {
            [OneTimeTearDown]
            public void TestFixtureTearDown()
            {
            }

            [Test]
            [Ignore("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
            public void Test()
            {
            }
        }
    }
}