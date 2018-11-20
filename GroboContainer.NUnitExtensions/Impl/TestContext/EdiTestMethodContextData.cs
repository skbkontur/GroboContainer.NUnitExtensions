using System;
using System.Collections.Generic;

using GroboContainer.Core;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl.TestContext
{
    public class EdiTestMethodContextData : EdiTestContextData
    {
        public EdiTestMethodContextData([NotNull] Lazy<IContainer> lazyContainer)
            : base(lazyContainer)
        {
            SetUppedMethodWrappers = new HashSet<EdiTestMethodWrapperAttribute>();
            IsSetUpped = false;
        }

        public HashSet<EdiTestMethodWrapperAttribute> SetUppedMethodWrappers { get; }

        public bool IsSetUpped { get; set; }
    }
}