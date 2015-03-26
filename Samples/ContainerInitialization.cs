using System;
using System.Reflection;

using GroboContainer.Core;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.CommonWrappers.ForSuite;
using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;
using SKBKontur.Catalogue.NUnit.Extensions.TestEnvironments.PropertyInjection;

#pragma warning disable 649

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Samples
{
    public interface IService1
    {
        void Foo();
    }

    public class Service1 : IService1
    {
        public void Foo()
        {
            Console.Out.WriteLine("Service1.Foo()");
        }
    }

    public interface IService2
    {
        void Hoo();
    }

    public class Service2 : IService2
    {
        private readonly string param;

        public Service2(string param)
        {
            this.param = param;
        }

        public void Hoo()
        {
            Console.Out.WriteLine("Service2.Hoo({0})", param);
        }
    }

    [WithContainerPerSuite]
    public class WithService2 : EdiTestSuiteWrapperAttribute
    {
        private readonly string p;

        protected override string TryGetIdentity()
        {
            return p;
        }

        public WithService2(string p)
        {
            this.p = p;
        }

        public override void SetUp(string testSuiteName, Assembly testAssembly, IEdiTestContextData testSuiteContext)
        {
            Console.Out.WriteLine("WithService2(p ={0}).SetUp()", p);
            testSuiteContext.GetContainer().Configurator.ForAbstraction<IService2>().UseInstances(new Service2(p));
        }
    }

    public class WithX : EdiTestSuiteWrapperAttribute
    {
        private readonly string p;

        protected override string TryGetIdentity()
        {
            return p;
        }

        public WithX(string p)
        {
            this.p = p;
        }

        public override void SetUp(string testSuiteName, Assembly testAssembly, IEdiTestContextData testSuiteContext)
        {
            Console.Out.WriteLine("WithX(p={0}).SetUp()", p);
        }
    }

    [WithX("0")]
    public class WithY : EdiTestSuiteWrapperAttribute
    {
        private readonly string p;

        protected override string TryGetIdentity()
        {
            return p;
        }

        public WithY(string p)
        {
            this.p = p;
        }

        public override void SetUp(string testSuiteName, Assembly testAssembly, IEdiTestContextData testSuiteContext)
        {
            Console.Out.WriteLine("WithY(p={0}).SetUp()", p);
        }
    }

    [WithY("0")]
    public class WithZ : EdiTestSuiteWrapperAttribute
    {
        private readonly string p;

        protected override string TryGetIdentity()
        {
            return p;
        }

        public WithZ(string p)
        {
            this.p = p;
        }

        public override void SetUp(string testSuiteName, Assembly testAssembly, IEdiTestContextData testSuiteContext)
        {
            Console.Out.WriteLine("WithZ(p={0}).SetUp()", p);
        }
    }



    [EdiTestFixture, WithContainerPerSuite, WithLogging]
    public class BaseTest
    {
        [Injected]
        private readonly IService1 service1;

        [Test]
        public void Test()
        {
            service1.Foo();
        }
    }

    [EdiTestSuite(), WithContainerPerSuite, WithLogging, WithService2("1")]
    public class Service2Test
    {
        private IService2 service2;

        [EdiSetUp]
        public void SetUp()
        {
            service2 = EdiTestContext.Current.Container.Get<IService2>();
        }

        [Test]
        public void Test()
        {
            service2.Hoo();
        }
    }

    [EdiTestFixture, WithService2("3"), WithZ("0")]
    public class Service2Test2
    {
        private IService2 service2;

        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp([NotNull] IContainer container)
        {
            Console.Out.WriteLine("TF1.TestFixtureSetUp");
        }

        [EdiSetUp]
        public void SetUp()
        {
            service2 = EdiTestContext.Current.Container.Get<IService2>();
        }

        [Test]
        public void Test()
        {
            service2.Hoo();
        }
    }

    [EdiTestFixture, WithContainerPerSuite, WithLogging]
    public class Service2Test3
    {
        private IService2 service2;

        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp([NotNull] IContainer container)
        {
            Console.Out.WriteLine("Service2Test3.TestFixtureSetUp");
            container.Configurator.ForAbstraction<IService2>().UseInstances(new Service2("5"));
        }

        [EdiSetUp]
        public void SetUp()
        {
            service2 = EdiTestContext.Current.Container.Get<IService2>();
        }

        [Test]
        public void Test()
        {
            service2.Hoo();
        }
    }
}