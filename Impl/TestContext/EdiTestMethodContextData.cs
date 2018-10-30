using System;
using System.Collections.Generic;

using GroboContainer.Core;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public class EdiTestMethodContextData : EdiTestContextData
    {
        public EdiTestMethodContextData([NotNull] Lazy<IContainer> lazyContainer)
            : base(lazyContainer)
        {
            SetUpedMethodWrappers = new HashSet<EdiTestMethodWrapperAttribute>();
            IsSetUped = false;
        }

        public HashSet<EdiTestMethodWrapperAttribute> SetUpedMethodWrappers { get; }
        public bool IsSetUped { get; set; }
    }
}