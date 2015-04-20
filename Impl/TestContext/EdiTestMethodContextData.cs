using System;

using GroboContainer.Core;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public class EdiTestMethodContextData : EdiTestContextData
    {
        public EdiTestMethodContextData([NotNull] Lazy<IContainer> lazyContainer)
            : base(lazyContainer)
        {
        }
    }
}