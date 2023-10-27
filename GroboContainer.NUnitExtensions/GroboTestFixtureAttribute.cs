using System;
using System.Diagnostics.CodeAnalysis;

using GroboContainer.NUnitExtensions.Impl.SetupAttributes;

namespace GroboContainer.NUnitExtensions
{
    [WithGroboContainer, AndGroboSetUpMethods]
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GroboTestFixtureAttribute : GroboTestSuiteAttributeBase
    {
    }
}