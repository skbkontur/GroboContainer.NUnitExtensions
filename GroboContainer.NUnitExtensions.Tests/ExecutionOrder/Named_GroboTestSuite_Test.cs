using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestSuite("NamedSuite")]
    public class Named_GroboTestSuite_Test : GroboTestMachineryTestBase
    {
        [Test]
        public void Test01()
        {
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(GroboTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test01"));
        }

        [TestCase("C1")]
        [TestCase("C2")]
        public void Test02(string @case)
        {
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(GroboTestContext.Current.TestName(), Is.EqualTo($"{GetType().FullName}.Test02(\"{@case}\")"));
        }

        [TestCase]
        public void Test03()
        {
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(GroboTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test03()"));
        }

        [Test]
        [Repeat(2)]
        public void Test04()
        {
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(GroboTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test04"));
        }
    }
}