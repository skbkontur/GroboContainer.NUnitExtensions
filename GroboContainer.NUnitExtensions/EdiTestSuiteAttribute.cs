using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class EdiTestSuiteAttribute : EdiTestSuiteAttributeBase
    {
        public EdiTestSuiteAttribute([NotNull] string suiteName = "DefaultSuite")
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