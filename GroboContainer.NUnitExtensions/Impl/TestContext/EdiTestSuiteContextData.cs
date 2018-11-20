using System;

using GroboContainer.Core;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TestContext
{
    public class EdiTestSuiteContextData : EdiTestContextData
    {
        public EdiTestSuiteContextData([NotNull] Lazy<IContainer> lazyContainer)
            : base(lazyContainer)
        {
        }
    }
}