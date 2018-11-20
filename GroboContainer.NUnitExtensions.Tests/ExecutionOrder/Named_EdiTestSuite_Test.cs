using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [EdiTestSuite("NamedSuite")]
    public class Named_EdiTestSuite_Test : EdiTestMachineryTestBase
    {
        [Test]
        public void Test01()
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(EdiTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test01"));
        }

        [TestCase("C1")]
        [TestCase("C2")]
        public void Test02(string @case)
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(EdiTestContext.Current.TestName(), Is.EqualTo($"{GetType().FullName}.Test02(\"{@case}\")"));
        }

        [TestCase]
        public void Test03()
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(EdiTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test03()"));
        }

        [Test]
        [Repeat(2)]
        public void Test04()
        {
            Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("NamedSuite"));
            Assert.That(EdiTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test04"));
        }
    }
}