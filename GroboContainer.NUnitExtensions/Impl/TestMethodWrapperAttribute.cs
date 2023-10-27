using System;
using System.Diagnostics.CodeAnalysis;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework.Interfaces;

using NotNullAttribute = JetBrains.Annotations.NotNullAttribute;

namespace GroboContainer.NUnitExtensions.Impl
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class TestMethodWrapperAttribute : GroboTestWrapperAttribute
    {
        public virtual void SetUp([NotNull] ITest test, [NotNull] IEditableGroboTestContext suiteContext, [NotNull] IEditableGroboTestContext methodContext)
        {
        }

        public virtual void TearDown([NotNull] ITest test, [NotNull] IEditableGroboTestContext suiteContext, [NotNull] IEditableGroboTestContext methodContext)
        {
        }
    }
}