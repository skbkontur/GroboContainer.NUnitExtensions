namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.Container
{
    public class ServiceDependingOnString : IServiceDependingOnString
    {
        public ServiceDependingOnString(string param)
        {
            this.param = param;
        }

        public void Hoo(int q)
        {
            EdiTestMachineryTrace.Log(string.Format("ServiceDependingOnString.Hoo(p={0}, q={1})", param, q));
        }

        private readonly string param;
    }
}