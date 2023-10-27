using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using GroboContainer.NUnitExtensions.Impl;
using GroboContainer.NUnitExtensions.Impl.SetupAttributes;
using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework.Interfaces;

using NotNullAttribute = JetBrains.Annotations.NotNullAttribute;

namespace GroboContainer.NUnitExtensions
{
    [WithGroboContainer]
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class GroboTestMethodWrapperAttribute : TestMethodWrapperAttribute
    {
        public override sealed IEnumerable<GroboTestWrapperAttribute> DependsOn()
        {
            return GetType().GetCustomAttributes(typeof(GroboTestWrapperAttribute), true).Cast<GroboTestWrapperAttribute>();
        }

        public override sealed void SetUp([NotNull] ITest test, [NotNull] IEditableGroboTestContext suiteContext, [NotNull] IEditableGroboTestContext methodContext)
        {
            SetUp(test.FullName, suiteContext, methodContext);
        }

        public override sealed void TearDown([NotNull] ITest test, [NotNull] IEditableGroboTestContext suiteContext, [NotNull] IEditableGroboTestContext methodContext)
        {
            TearDown(test.FullName, suiteContext, methodContext);
        }

        public virtual void SetUp([NotNull] string testName, [NotNull] IEditableGroboTestContext suiteContext, [NotNull] IEditableGroboTestContext methodContext)
        {
        }

        public virtual void TearDown([NotNull] string testName, [NotNull] IEditableGroboTestContext suiteContext, [NotNull] IEditableGroboTestContext methodContext)
        {
        }
    }
}