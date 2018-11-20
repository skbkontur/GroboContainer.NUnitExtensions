using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
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