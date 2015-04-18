namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container
{
    public class ServiceWithNoDependencies : IServiceWithNoDependencies
    {
        public void Foo(int p)
        {
            EdiTestMachineryTrace.Log(string.Format("ServiceWithNoDependencies.Foo(p={0})", p));
        }
    }
}