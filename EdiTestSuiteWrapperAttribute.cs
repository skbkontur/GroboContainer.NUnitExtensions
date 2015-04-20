using System;
using System.Reflection;

using JetBrains.Annotations;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public abstract class EdiTestSuiteWrapperAttribute : EdiTestWrapperAttribute
    {
        public virtual void SetUp([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEditableEdiTestContext suiteContext)
        {
        }

        public virtual void TearDown([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEditableEdiTestContext suiteContext)
        {
        }
    }
}