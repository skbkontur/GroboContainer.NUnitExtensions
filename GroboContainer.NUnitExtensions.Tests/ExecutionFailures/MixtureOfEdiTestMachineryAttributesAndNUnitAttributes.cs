using FluentAssertions;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionFailures
{
    [TestFixture]
    [EdiTestSuite]
    [Explicit("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
    public class NUnit_TestFixtureAttribute_IsProhibited_ExplicitTest
    {
        [Test]
        public void Test()
        {
        }
    }

    [EdiTestSuite]
    [Explicit("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
    public class NUnit_SetUpAttribute_IsProhibited_ExplicitTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Test()
        {
        }
    }

    [EdiTestSuite]
    [Explicit("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
    public class NUnit_TearDownAttribute_IsProhibited_ExplicitTest
    {
        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Test()
        {
        }
    }

    [EdiTestSuite]
    [Explicit("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
    public class NUnit_TestFixtureSetUpAttribute_IsProhibited_ExplicitTest
    {
        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
        }

        [Test]
        public void Test()
        {
        }
    }

    [EdiTestSuite]
    [Explicit("Intentionally fails with 'Prohibited NUnit attributes ...' error")]
    public class NUnit_TestFixtureTearDownAttribute_IsProhibited_ExplicitTest
    {
        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
        }

        [Test]
        public void Test()
        {
        }
    }

    public class NUnitProhibitedAttributesTest
    {
        [Test]
        [Ignore("p.vostretsov, 19.10.2018: Turns out, TestFixtureAttribute is not prohibited, does it need to be?")]
        public void NUnit_TestFixtureAttribute_IsProhibited_Test()
        {
            var testResults = TestRunner.RunTestClass<NUnit_TestFixtureAttribute_IsProhibited_ExplicitTest>();
            var result = testResults[nameof(NUnit_TestFixtureAttribute_IsProhibited_ExplicitTest.Test)];
            result.Message.Should().Contain("Prohibited NUnit attributes (SetUpAttribute, TearDownAttribute, OneTimeSetUpAttribute, OneTimeTearDownAttribute) are used in:");
        }

        [Test]
        public void NUnit_SetUpAttribute_IsProhibited_Test()
        {
            var testResults = TestRunner.RunTestClass<NUnit_SetUpAttribute_IsProhibited_ExplicitTest>();
            var result = testResults[nameof(NUnit_SetUpAttribute_IsProhibited_ExplicitTest.Test)];
            result.Message.Should().Contain("Prohibited NUnit attributes (SetUpAttribute, TearDownAttribute, OneTimeSetUpAttribute, OneTimeTearDownAttribute) are used in:");
        }

        [Test]
        public void NUnit_TearDownAttribute_IsProhibited_Test()
        {
            var testResults = TestRunner.RunTestClass<NUnit_TearDownAttribute_IsProhibited_ExplicitTest>();
            var result = testResults[nameof(NUnit_TearDownAttribute_IsProhibited_ExplicitTest.Test)];
            result.Message.Should().Contain("Prohibited NUnit attributes (SetUpAttribute, TearDownAttribute, OneTimeSetUpAttribute, OneTimeTearDownAttribute) are used in:");
        }

        [Test]
        public void NUnit_TestFixtureSetUpAttribute_IsProhibited_Test()
        {
            var testResults = TestRunner.RunTestClass<NUnit_TestFixtureSetUpAttribute_IsProhibited_ExplicitTest>();
            var result = testResults[nameof(NUnit_TestFixtureSetUpAttribute_IsProhibited_ExplicitTest.Test)];
            result.Message.Should().Contain("Prohibited NUnit attributes (SetUpAttribute, TearDownAttribute, OneTimeSetUpAttribute, OneTimeTearDownAttribute) are used in:");
        }

        [Test]
        public void NUnit_TestFixtureTearDownAttribute_IsProhibited_Test()
        {
            var testResults = TestRunner.RunTestClass<NUnit_TestFixtureTearDownAttribute_IsProhibited_ExplicitTest>();
            var result = testResults[nameof(NUnit_TestFixtureTearDownAttribute_IsProhibited_ExplicitTest.Test)];
            result.Message.Should().Contain("Prohibited NUnit attributes (SetUpAttribute, TearDownAttribute, OneTimeSetUpAttribute, OneTimeTearDownAttribute) are used in:");
        }
    }
}