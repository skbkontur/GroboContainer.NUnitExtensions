using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
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
                        $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                        $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
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
                        $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                        $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        "Part01_SetUp()",
                        "Test01()",
                        "Part01_TearDown()",
                        $"MethodWrapper.TearDown() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02",
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
                        $"SuiteWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}",
                        $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        "Part01_SetUp()",
                        "Test01()",
                        "Part01_TearDown()",
                        $"MethodWrapper.TearDown() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02",
                        "Part02_SetUp()",
                        "Test02()",
                        "Part02_TearDown()",
                        $"MethodWrapper.TearDown() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02",
                        $"MethodWrapper.SetUp() for {EdiTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test03",
                        "Part02_SetUp()",
                        "Test03()",
                    }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
            }
        }
    }
}