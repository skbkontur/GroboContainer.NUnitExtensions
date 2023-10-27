using System.Collections.Generic;

namespace GroboContainer.NUnitExtensions.Impl.TestContext
{
    public class GroboTestMethodContextData : GroboTestContextData
    {
        public HashSet<TestMethodWrapperAttribute> SetUppedMethodWrappers { get; } = new HashSet<TestMethodWrapperAttribute>();
    }
}