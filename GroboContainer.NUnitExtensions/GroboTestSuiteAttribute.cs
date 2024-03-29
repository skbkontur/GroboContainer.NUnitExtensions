using System;
using System.Diagnostics.CodeAnalysis;

using NotNullAttribute = JetBrains.Annotations.NotNullAttribute;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class GroboTestSuiteAttribute : GroboTestSuiteAttributeBase
    {
        public GroboTestSuiteAttribute([NotNull] string suiteName = "DefaultSuite")
        {
            SuiteName = suiteName;
        }

        [NotNull]
        public string SuiteName { get; }

        public override string ToString()
        {
            return $"SuiteName: {SuiteName}";
        }
    }
}