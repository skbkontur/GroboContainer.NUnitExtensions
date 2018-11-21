using System;
using System.Diagnostics.CodeAnalysis;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GroboTestFixtureAttribute : GroboTestSuiteAttributeBase
    {
    }
}