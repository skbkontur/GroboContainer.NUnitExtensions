namespace GroboContainer.NUnitExtensions.Tests.Container
{
    public class ServiceWithNoDependencies : IServiceWithNoDependencies
    {
        public void Foo(int p)
        {
            EdiTestMachineryTrace.Log(string.Format("ServiceWithNoDependencies.Foo(p={0})", p));
        }
    }
}