using System;
using System.Collections.Generic;

using GroboContainer.Core;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TestContext
{
    public class GroboTestMethodContextData : GroboTestContextData
    {
        public GroboTestMethodContextData([NotNull] Lazy<IContainer> lazyContainer)
            : base(lazyContainer)
        {
            SetUppedMethodWrappers = new HashSet<GroboTestMethodWrapperAttribute>();
            IsSetUpped = false;
        }

        public HashSet<GroboTestMethodWrapperAttribute> SetUppedMethodWrappers { get; }

        public bool IsSetUpped { get; set; }
    }
}