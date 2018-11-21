using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [TestFixture]
    public class SharedSuiteContext_Test
    {
        private static void AssertTestMachineryTrace(string[] expectedMessages)
        {
            Assert.That(GroboTestMachineryTrace.TraceLines, Is.EqualTo(expectedMessages));
        }

        [GroboTestSuite("SharedSuiteContext"), WithDebugLogPerSuite, AndDebugLogPerMethod]
        public class Part01
        {
            public Part01()
            {
                GroboTestMachineryTrace.ClearTrace();
            }

            [GroboSetUp]
            public void SetUp()
            {
                GroboTestMachineryTrace.Log("Part01_SetUp()");
            }

            [GroboTearDown]
            public void TearDown()
            {
                GroboTestMachineryTrace.Log("Part01_TearDown()");
            }

            [Test]
            public void Test01()
            {
                Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("SharedSuiteContext"));
                GroboTestMachineryTrace.Log("Test01()");
                AssertTestMachineryTrace(new[]
                    {
                        $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                        $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        "Part01_SetUp()",
                        "Test01()",
                    });
            }
        }

        [GroboTestSuite("SharedSuiteContext"), WithDebugLogPerSuite, AndDebugLogPerMethod]
        public class Part02
        {
            [GroboSetUp]
            public void SetUp()
            {
                GroboTestMachineryTrace.Log("Part02_SetUp()");
            }

            [GroboTearDown]
            public void TearDown()
            {
                GroboTestMachineryTrace.Log("Part02_TearDown()");
            }

            [Test]
            public void Test02()
            {
                Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("SharedSuiteContext"));
                GroboTestMachineryTrace.Log("Test02()");
                AssertTestMachineryTrace(new[]
                    {
                        $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                        $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        "Part01_SetUp()",
                        "Test01()",
                        "Part01_TearDown()",
                        $"MethodWrapper.TearDown() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02",
                        "Part02_SetUp()",
                        "Test02()",
                    }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
            }

            [Test]
            public void Test03()
            {
                Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo("SharedSuiteContext"));
                GroboTestMachineryTrace.Log("Test03()");
                AssertTestMachineryTrace(new[]
                    {
                        $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                        $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        "Part01_SetUp()",
                        "Test01()",
                        "Part01_TearDown()",
                        $"MethodWrapper.TearDown() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part01.Test01",
                        $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02",
                        "Part02_SetUp()",
                        "Test02()",
                        "Part02_TearDown()",
                        $"MethodWrapper.TearDown() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test02",
                        $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.SharedSuiteContext_Test+Part02.Test03",
                        "Part02_SetUp()",
                        "Test03()",
                    }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
            }
        }
    }
}