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
            SetUpWrappers = new HashSet<EdiTestMethodWrapperAttribute>();
            IsSetUp = false;
        }

        public HashSet<EdiTestMethodWrapperAttribute> SetUpWrappers { get; }
        public bool IsSetUp { get; set; }
    }
}