using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using GroboContainer.NUnitExtensions.Impl;
using GroboContainer.NUnitExtensions.Impl.SetupAttributes;

namespace GroboContainer.NUnitExtensions
{
    [WithGroboContainer]
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public abstract class GroboTestSuiteWrapperAttribute : TestSuiteWrapperAttribute
    {
        public override sealed IEnumerable<GroboTestWrapperAttribute> DependsOn()
        {
            return GetType().GetCustomAttributes(typeof(GroboTestWrapperAttribute), true).Cast<GroboTestWrapperAttribute>();
        }
    }
}