namespace GroboContainer.NUnitExtensions.Tests.Container
{
    public class ServiceDependingOnString : IServiceDependingOnString
    {
        public ServiceDependingOnString(string param)
        {
            this.param = param;
        }

        public void Hoo(int q)
        {
            GroboTestMachineryTrace.Log($"ServiceDependingOnString.Hoo(p={param}, q={q})");
        }

        private readonly string param;
    }
}