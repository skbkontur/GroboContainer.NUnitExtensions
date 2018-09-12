using System.Reflection;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Parallel
{
    public class Counter
    {
        public int InvocationsCount { get; set; }
    }

    public class WithTestInvocationCounter : EdiTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableEdiTestContext suiteContext)
        {
            suiteContext.AddItem("counter", new Counter());
        }
    }

    public class AndTestInvocationCounter : EdiTestMethodWrapperAttribute
    {
        public override void SetUp(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            methodContext.AddItem("counter", new Counter());
        }
    }

    [EdiTestSuite, WithTestInvocationCounter, AndTestInvocationCounter, Parallelizable(ParallelScope.All)]
    public class ParallelTestContextUsageTest2
    {
        [Test]
        public void Test()
        {
            ParallelTestContextUsageTest.TestInvocationCount();
        }
    }

    [EdiTestSuite("Diff"), WithTestInvocationCounter, AndTestInvocationCounter, Parallelizable(ParallelScope.All)]
    public class ParallelTestContextUsageTest
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
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

        public static void TestInvocationCount()
        {
            Assert.That(EdiTestContext.Current.TryGetContextItem("counter", out var o), Is.True);
            var counter = (Counter)o;
            counter.InvocationsCount++;
            Assert.That(counter.InvocationsCount, Is.EqualTo(1));
        }

        private static readonly TestCaseData[] testCases =
            {
                new TestCaseData(),
                new TestCaseData(),
            };
    }
}