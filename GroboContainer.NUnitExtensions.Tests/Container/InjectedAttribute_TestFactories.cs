using System;
using System.Linq;

using JetBrains.Annotations;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [GroboTestFixture]
    public class InjectedAttribute_TestFactories : InjectedAttributeTestBase
    {
        [GroboSetUp]
        public void SetUp()
        {
            GroboTestMachineryTrace.ClearTrace();
        }

        [Test]
        public void TestSimpleFactory()
        {
            var service = factory();
            Assert.That(service, Is.Not.Null);
            service.Foo(41);
            Assert.That(GroboTestMachineryTrace.TraceLines, Is.EqualTo(new[] {"ServiceWithNoDependencies.Foo(p=41)"}));
        }

        [Test]
        public void TestFactoryFromString()
        {
            var service = factoryFromString("qxx");
            Assert.That(service, Is.Not.Null);
            service.Hoo(42);
            Assert.That(GroboTestMachineryTrace.TraceLines, Is.EqualTo(new[] {"ServiceDependingOnString.Hoo(p=qxx, q=42)"}));
        }

        [Test]
        public void TestFactory4()
        {
            var service = factory4(new[] {"qxx"}, new[] {1}, new short[] {2}, new byte[] {3});
            Assert.That(service, Is.Not.Null);
            service.Qoo(43);
            Assert.That(GroboTestMachineryTrace.TraceLines, Is.EqualTo(new[] {"ServiceDependingOnManyStrings.Qoo(a=qxx, b=1, c=2, d=3, p=43)"}));
        }

        [Injected]
        private Func<IServiceWithNoDependencies> factory;

        [Injected]
        private Func<string, IServiceDependingOnString> factoryFromString;

        [Injected]
        private Func<string[], int[], short[], byte[], IServiceWithManyDependencies> factory4;
    }

    public interface IServiceWithManyDependencies
    {
        void Qoo(int p);
    }

    public class ServiceWithManyDependencies : IServiceWithManyDependencies
    {
        private readonly string[] a;
        private readonly int[] b;
        private readonly short[] c;
        private readonly byte[] d;

        public ServiceWithManyDependencies(string[] a, int[] b, short[] c, byte[] d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public void Qoo(int p)
        {
            GroboTestMachineryTrace.Log($"ServiceDependingOnManyStrings.Qoo(" +
                                        $"a={(a == null ? "null" : string.Join(", ", a))}, " +
                                        $"b={(b == null ? "null" : string.Join(", ", b))}, " +
                                        $"c={(c == null ? "null" : string.Join(", ", c))}, " +
                                        $"d={(d == null ? "null" : string.Join(", ", d))}, " +
                                        $"p={p})");
        }
    }
}