using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container
{
    public class InjectedAttributeTestBase
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