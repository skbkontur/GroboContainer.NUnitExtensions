using System.Linq;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Parallel
{
    public class Counter
    {
        public int InvocationsCount { get; set; }
    }

    public class AndTestInvocationCounter : EdiTestMethodWrapperAttribute
    {
        public override void SetUp(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            methodContext.AddItem("counter", new Counter());
        }
    }

    [EdiTestFixture, AndTestInvocationCounter, Parallelizable(ParallelScope.Children)]
    public class ParallelTestContextUsageTest
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public void Test(int n)
        {
            TestInvocationCount();
        }

        [Test]
        public void Test1()
        {
            TestInvocationCount();
        }

        [Test]
        public void Test2()
        {
            TestInvocationCount();
        }

        [TestCaseSource(nameof(testCases))]
        public void TestSource()
        {
            TestInvocationCount();
        }

        private static void TestInvocationCount()
        {
            Assert.That(EdiTestContext.Current.TryGetContextItem("counter", out var o), Is.True);
            var counter = (Counter)o;
            counter.InvocationsCount++;
            Assert.That(counter.InvocationsCount, Is.EqualTo(1));
        }

        private static readonly TestCaseData[] testCases = Enumerable.Range(0, 100).Select(x => new TestCaseData()).ToArray();
    }
}