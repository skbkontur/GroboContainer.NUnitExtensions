using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container.InjectionHardTests
{
    [EdiTestFixture]
    [TestFixture(1)]
    [TestFixture(2)]
    public class InjectedAttributeInjectedOnlyOncePerInstanceWithTestClassParamsTest : InjectedAttributeTestBase
    {
        public InjectedAttributeInjectedOnlyOncePerInstanceWithTestClassParamsTest(int x)
        {
            this.x = x;
        }

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
            switch (x)
            {
            case 1:
                if (ServiceForTest.wasNotNull1)
                    Assert.IsNull(injected, "double injected");
                else
                {
                    Assert.IsNotNull(injected, "instance not injected !!");
                    ServiceForTest.wasNotNull1 = true;
                    injected = null;
                }
                break;
            case 2:
                if (ServiceForTest.wasNotNull2)
                    Assert.IsNull(injected, "double injected");
                else
                {
                    Assert.IsNotNull(injected, "instance not injected !!");
                    ServiceForTest.wasNotNull2 = true;
                    injected = null;
                }
                break;
            default:
                Assert.Fail("Unknown parameter value {0}", x);
                break;
            }
        }

        [Injected]
        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once UnassignedReadonlyField
        // ReSharper disable once MemberCanBePrivate.Global
        private ServiceForTest injected;

        private readonly int x;

        public class ServiceForTest
        {
            public static bool wasNotNull1;
            public static bool wasNotNull2;
        }
    }
}