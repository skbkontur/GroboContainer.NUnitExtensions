using System.IO;
using System.Reflection;

using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    public static class Log
    {
        public static void Clear()
        {
            if (File.Exists(filename))
                File.Delete(filename);
        }

        public static void Line(string line)
        {
            File.AppendAllLines(filename, new[] {line});
        }

        public static void Check(string[] expectedLines)
        {
            File.ReadAllLines(filename).Should().BeEquivalentTo(expectedLines);
        }

        private static readonly string filename = TestContext.CurrentContext.TestDirectory + "/GlobalTearDown.log";
    }

    public class AndLocalTearDown : GroboTestMethodWrapperAttribute
    {
        public override void SetUp(string testName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            Log.Line("AndLocalTearDown.SetUp");
        }

        public override void TearDown(string testName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            Log.Line("AndLocalTearDown.TearDown");
        }
    }

    [AndLocalTearDown]
    public class WithGlobalTearDown : GroboTestSuiteWrapperAttribute
    {
        public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            Log.Clear();
            Log.Line("WithGlobalTearDown.SetUp");
        }

        public override void TearDown(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
        {
            Log.Line("WithGlobalTearDown.TearDown");
        }
    }

    [GroboTestSuite("GlobalTearDown"), WithGlobalTearDown]
    public class GlobalTearDownTest
    {
        [Test]
        public void Test()
        {
            Log.Line("GlobalTearDownTest.Test");
        }
    }

    [Explicit]
    public class AfterTestCheck
    {
        [Test]
        public void Test()
        {
            Log.Check(new[]
                {
                    "WithGlobalTearDown.SetUp",
                    "AndLocalTearDown.SetUp",
                    "GlobalTearDownTest.Test",
                    "AndLocalTearDown.TearDown",
                    "WithGlobalTearDown.TearDown",
                });
        }
    }
}