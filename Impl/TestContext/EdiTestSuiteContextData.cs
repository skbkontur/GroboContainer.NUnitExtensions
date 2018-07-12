using System;

using GroboContainer.Core;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public class EdiTestSuiteContextData : EdiTestContextData
    {
        public EdiTestSuiteContextData([NotNull] Lazy<IContainer> lazyContainer)
            : base(lazyContainer)
        {
        }
    }
}