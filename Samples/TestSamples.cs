using System;

using NUnit.Framework;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Samples
{
    [EdiTestSuite("Sample"), WithDebugLogPerSuite, WithDebugLogPerMethod]
    public class TF1
    {
        [EdiSetUp]
        public void SetUp()
        {
            Console.Out.WriteLine("TF1.SetUp: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [EdiTearDown]
        public void TearDown()
        {
            Console.Out.WriteLine("TF1.TearDown: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [Test]
        public void F1_T1()
        {
            Console.Out.WriteLine("TF1.F1_T1: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [TestCase("C1")]
        [TestCase("C2")]
        public void F1_T2(string @case)
        {
            Console.Out.WriteLine("TF1.F1_T2: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [TestCase]
        public void F1_T3()
        {
            Console.Out.WriteLine("TF1.F1_T3: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [Test]
        [Repeat(2)]
        public void F1_T4()
        {
            Console.Out.WriteLine("TF1.F1_T4: {0}", EdiTestContext.Current.SuiteDebugId());
        }
    }

    [Ignore("todo [edi-test]: fails because of multiple [TestSuite] declarations")]
    [EdiTestSuite("Sample")]
    public class TF1_Derived : TF1
    {
        [Test]
        public void Test()
        {
            Console.Out.WriteLine("TF1_Derived.Test: {0}", EdiTestContext.Current.SuiteDebugId());
        }
    }

    [EdiTestSuite(Suite.SampleTests), WithDebugLogPerSuite, WithDebugLogPerMethod]
    public class TF2
    {
        [Test]
        public void F2_T1()
        {
            Console.Out.WriteLine("TF2.F2_T1: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [Test]
        public void F2_T2()
        {
            Console.Out.WriteLine("TF2.F2_T2: {0}", EdiTestContext.Current.SuiteDebugId());
        }
    }

    [EdiTestSuite]
    public class TF3 : ISampleTests
    {
        [Test]
        public void F3_T1()
        {
            Console.Out.WriteLine("TF3.F3_T1: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [Test]
        public void F3_T2()
        {
            Console.Out.WriteLine("TF3.F3_T2: {0}", EdiTestContext.Current.SuiteDebugId());
        }
    }

    [WithDebugLogPerSuite]
    public interface ISampleTests
    {
    }

    [EdiTestFixture, WithDebugLogPerMethod]
    public class TF4 : ISampleTests
    {
        [Test]
        public void F4_T1()
        {
            Console.Out.WriteLine("TF4.F4_T1: {0}", EdiTestContext.Current.SuiteDebugId());
        }

        [Test]
        public void F4_T2()
        {
            Console.Out.WriteLine("TF4.F4_T2: {0}", EdiTestContext.Current.SuiteDebugId());
        }
    }

    public static class Suite
    {
        public const string SampleTests = "Sample";
    }

    public static class EdiTestContextExtensions
    {
        public static Guid SuiteDebugId(this IEdiTestContext ctx)
        {
            return ctx.TryGetContextItem<Guid>("SuiteDebugId");
        }
    }
}