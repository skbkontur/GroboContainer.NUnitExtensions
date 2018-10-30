using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionFailures
{
    [EdiTestSuite("BaseSuite")]
    [Explicit("Intentionally fails with 'There are multiple suite names (BaseSuite, BaseSuite) defined for ...' error")]
    public class DerivedTestClass_WithItsOwnEdiTestSuiteAttribute_ExplicitTest : TestBaseWithEdiTestSuiteAttribute
    {
        [Test]
        public void Test()
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("BaseSuite"));
        }
    }

    public class DerivedTestClass_WithItsOwnEdiTestSuiteAttribute_Test
    {
        [Test]
        public void Test()
        {
            var testResults = TestRunner.RunTestClass<DerivedTestClass_WithItsOwnEdiTestSuiteAttribute_ExplicitTest>();
            var result = testResults[nameof(DerivedTestClass_WithItsOwnEdiTestSuiteAttribute_ExplicitTest.Test)];
            result.Message.Should().Contain("There are multiple suite names (BaseSuite, BaseSuite) defined for:");
        }
    }
}