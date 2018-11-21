using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container.InjectionHardTests
{
    [GroboTestFixture]
    public class InjectedAttributeInjectedOnlyOncePerInstanceTest : InjectedAttributeTestBase
    {
        [Test]
        public void Test1()
        {
            Check();
        }

        [Test]
        public void Test2()
        {
            Check();
        }

        private void Check()
        {
            if (ServiceForTest.wasNotNull)
                Assert.IsNull(injected, "double injected");
            else
            {
                Assert.IsNotNull(injected, "instance not injected !!");
                ServiceForTest.wasNotNull = true;
                injected = null;
            }
        }

        [Injected]
        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once UnassignedReadonlyField
        // ReSharper disable once MemberCanBePrivate.Global
        private ServiceForTest injected;

        public class ServiceForTest
        {
            public static bool wasNotNull;
        }
    }
}