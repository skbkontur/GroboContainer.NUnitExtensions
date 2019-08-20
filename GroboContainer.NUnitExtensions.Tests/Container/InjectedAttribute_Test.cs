using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    [GroboTestFixture]
    public class InjectedAttribute_Test : InjectedAttributeTestBase
    {
        [Test]
        public void PublicField()
        {
            Assert.That(publicField, Is.Not.Null);
        }

        [Test]
        public void PublicProperty_IsInjected()
        {
            Assert.That(PublicProperty, Is.Not.Null);
        }

        [Test]
        public void PrivateProperty_IsInjected()
        {
            Assert.That(PrivateProperty, Is.Not.Null);
        }

        [Test]
        public void ProtectedPropertyInBaseClass_IsInjected()
        {
            Assert.That(ProtectedPropertyInBaseClass, Is.Not.Null);
        }

        [Test]
        public void PrivateReadonlyField()
        {
            Assert.That(privateReadonlyField, Is.Not.Null);
        }

        [Test]
        public void ProtectedReadonlyFieldInBaseClass()
        {
            Assert.That(protectedReadonlyFieldInBaseClass, Is.Not.Null);
        }

        [Test]
        public void PrivateReadonlyFieldInBaseClass()
        {
            DoPrivateReadonlyFieldInBaseClass();
        }

        [Test]
        public void PrivatePropertyInBaseClass()
        {
            DoPrivatePropertyInBaseClass();
        }

        [Injected]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IServiceWithNoDependencies PublicProperty { get; set; }

        [Injected]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private IServiceWithNoDependencies PrivateProperty { get; set; }

#pragma warning disable 649
        [Injected]
        private readonly IServiceWithNoDependencies privateReadonlyField;
#pragma warning restore 649

        [Injected]
        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once UnassignedReadonlyField
        // ReSharper disable once MemberCanBePrivate.Global
        public IServiceWithNoDependencies publicField;
    }
}