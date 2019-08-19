using System.Threading.Tasks;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionOrder
{
    [GroboTestFixture]
    public class AsyncGroboTestFixture_Test : GroboTestMachineryTestBase
    {
        [GroboTestFixtureSetUp]
        public async Task TestFixtureSetUpAsync(IEditableGroboTestContext suiteContext)
        {
            await Task.Delay(1000);
            GroboTestMachineryTrace.Log("TestFixtureSetUp()");
        }

        [GroboSetUp]
        public async Task SetUpAsync()
        {
            await Task.Delay(1000);
            GroboTestMachineryTrace.Log("SetUp()");
        }

        [GroboTearDown]
        public async Task TearDownAsync()
        {
            await Task.Delay(1000);
            GroboTestMachineryTrace.Log("TearDown()");
        }

        [Test]
        public async Task Test01()
        {
            await Task.Delay(1000);
            Assert.That(GroboTestContext.Current.SuiteName(), Is.EqualTo(GetType().FullName));
            Assert.That(GroboTestContext.Current.TestName(), Is.EqualTo(GetType().FullName + ".Test01"));
            GroboTestMachineryTrace.Log("Test01()");
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "TestFixtureSetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::{GroboTestContext.Current.TestName()}",
                    "SetUp()",
                    "Test01()",
                });
        }

        [Test]
        public async Task Test02()
        {
            await Task.Delay(1000);
            GroboTestMachineryTrace.Log("Test02()");
            AssertTestMachineryTrace(new[]
                {
                    $"SuiteWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}",
                    "TestFixtureSetUp()",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.AsyncGroboTestFixture_Test.Test01",
                    "SetUp()",
                    "Test01()",
                    "TearDown()",
                    $"MethodWrapper.TearDown() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.AsyncGroboTestFixture_Test.Test01",
                    $"MethodWrapper.SetUp() for {GroboTestContext.Current.SuiteName()}::GroboContainer.NUnitExtensions.Tests.ExecutionOrder.AsyncGroboTestFixture_Test.Test02",
                    "SetUp()",
                    "Test02()",
                }); // NB! полагаемся на алфавитный порядок запуска тестов внутри одного класса
        }
    }
}