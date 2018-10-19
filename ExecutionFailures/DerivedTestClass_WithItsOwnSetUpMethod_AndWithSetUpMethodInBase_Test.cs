using FluentAssertions;

using NUnit.Framework;

using SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionFailures
{
    [EdiTestSuite("InheritanceHierarchyForSetUpMethod2")]
    [Explicit("Intentionally fails with 'There are multiple methods marked with EdiSetUp/EdiTearDown attribute in ...' error")]
    public class DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest : TestBaseWithSetUpMethod
    {
        [EdiSetUp]
        public void SetUp()
        {
            EdiTestMachineryTrace.Log("SetUp()");
        }

        [EdiTearDown]
        public void TearDown()
        {
            EdiTestMachineryTrace.Log("TearDown()");
        }

        [Test]
        public void Test01()
        {
            EdiTestMachineryTrace.Log("Test01()");
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("InheritanceHierarchyForSetUpMethod"));
        }
    }

    public class DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_Test
    {
        [Test]
        public void Test01()
        {
            var testResults = TestRunner.RunTestClass<DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest>();
            var result = testResults[nameof(DerivedTestClass_WithItsOwnSetUpMethod_AndWithSetUpMethodInBase_ExplicitTest.Test01)];
            result.Message.Should().StartWith("SKBKontur.Catalogue.Objects.InvalidProgramStateException : There are multiple methods marked with EdiSetUpAttribute attribute in:");
        }
    }
}