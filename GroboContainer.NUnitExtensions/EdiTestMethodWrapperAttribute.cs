using System;
using System.Diagnostics.CodeAnalysis;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class EdiTestMethodWrapperAttribute : EdiTestWrapperAttribute
    {
        public virtual void SetUp([NotNull] string testName, [NotNull] IEditableEdiTestContext suiteContext, [NotNull] IEditableEdiTestContext methodContext)
        {
        }

        public virtual void TearDown([NotNull] string testName, [NotNull] IEditableEdiTestContext suiteContext, [NotNull] IEditableEdiTestContext methodContext)
        {
        }
    }
}