using System;

using GroboContainer.Core;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TestContext
{
    public class GroboTestSuiteContextData : GroboTestContextData
    {
        public GroboTestSuiteContextData([NotNull] Lazy<IContainer> lazyContainer)
            : base(lazyContainer)
        {
        }
    }
}