namespace GroboContainer.NUnitExtensions.Tests.Container
{
    public class ServiceWithNoDependencies : IServiceWithNoDependencies
    {
        public void Foo(int p)
        {
            GroboTestMachineryTrace.Log($"ServiceWithNoDependencies.Foo(p={p})");
        }
    }
}