using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NotNullAttribute = JetBrains.Annotations.NotNullAttribute;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public abstract class GroboTestSuiteWrapperAttribute : GroboTestWrapperAttribute
    {
        public virtual void SetUp([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEditableGroboTestContext suiteContext)
        {
        }

        public virtual void TearDown([NotNull] string suiteName, [NotNull] Assembly testAssembly, [NotNull] IEditableGroboTestContext suiteContext)
        {
        }
    }
}