using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder
{
    [TestFixture]
    public class SharedSuiteContext_Test
    {
        private static void AssertEdiTestMachineryTrace(string[] expectedMessages)
        {
            Assert.That(EdiTestMachineryTrace.TraceLines, Is.EqualTo(expectedMessages));
        }

        [EdiTestSuite("SharedSuiteContext"), WithDebugLogPerSuite, AndDebugLogPerMethod]
        public class Part01
        {
            public Part01()
            {
                EdiTestMachineryTrace.ClearTrace();
            }

            [EdiSetUp]
            public void SetUp()
            {
                EdiTestMachineryTrace.Log("Part01_SetUp()");
            }

            [EdiTearDown]
            public void TearDown()
            {
                EdiTestMachineryTrace.Log("Part01_TearDown()");
            }

            [Test]
            public void Test01()
            {
                Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("SharedSuiteContext"));
                EdiTestMachineryTrace.Log("Test01()");
                AssertEdiTestMachineryTrace(new[]
                    {
                        string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                        string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01", EdiTestContext.Current.SuiteName()),
                        "Part01_SetUp()",
                        "Test01()",
                    });
            }
        }

        [EdiTestSuite("SharedSuiteContext"), WithDebugLogPerSuite, AndDebugLogPerMethod]
        public class Part02
        {
            [EdiSetUp]
            public void SetUp()
            {
                EdiTestMachineryTrace.Log("Part02_SetUp()");
            }

            [EdiTearDown]
            public void TearDown()
            {
                EdiTestMachineryTrace.Log("Part02_TearDown()");
            }

            [Test]
            public void Test02()
            {
                Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("SharedSuiteContext"));
                EdiTestMachineryTrace.Log("Test02()");
                AssertEdiTestMachineryTrace(new[]
                    {
                        string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                        string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01", EdiTestContext.Current.SuiteName()),
                        "Part01_SetUp()",
                        "Test01()",
                        "Part01_TearDown()",
                        string.Format("MethodWrapper.TearDown() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01", EdiTestContext.Current.SuiteName()),
                        string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02", EdiTestContext.Current.SuiteName()),
                        "Part02_SetUp()",
                        "Test02()",
                    }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
            }

            [Test]
            public void Test03()
            {
                Assert.That(EdiTestContext.Current.SuiteName(), Is.EqualTo("SharedSuiteContext"));
                EdiTestMachineryTrace.Log("Test03()");
                AssertEdiTestMachineryTrace(new[]
                    {
                        string.Format("SuiteWrapper.SetUp() for {0}", EdiTestContext.Current.SuiteName()),
                        string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01", EdiTestContext.Current.SuiteName()),
                        "Part01_SetUp()",
                        "Test01()",
                        "Part01_TearDown()",
                        string.Format("MethodWrapper.TearDown() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01", EdiTestContext.Current.SuiteName()),
                        string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02", EdiTestContext.Current.SuiteName()),
                        "Part02_SetUp()",
                        "Test02()",
                        "Part02_TearDown()",
                        string.Format("MethodWrapper.TearDown() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02", EdiTestContext.Current.SuiteName()),
                        string.Format("MethodWrapper.SetUp() for {0}::SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionOrder.SharedSuiteContext_Test+Part02.Test03", EdiTestContext.Current.SuiteName()),
                        "Part02_SetUp()",
                        "Test03()",
                    }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
            }
        }
    }
}