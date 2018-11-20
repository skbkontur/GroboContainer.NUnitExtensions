using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.Container
{
    public abstract class InjectedAttributeTestBase
    {
        protected void DoPrivateReadonlyFieldInBaseClass()
        {
            Assert.That(privateReadonlyFieldInBaseClass, Is.Not.Null);
        }

#pragma warning disable 649
        [Injected]
        private readonly IServiceWithNoDependencies privateReadonlyFieldInBaseClass;
#pragma warning restore 649

        [Injected]
        // ReSharper disable once UnassignedReadonlyField
        protected readonly IServiceWithNoDependencies protectedReadonlyFieldInBaseClass;
    }
}