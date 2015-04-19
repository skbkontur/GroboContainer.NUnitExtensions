using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
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
        public string SuiteName { get; private set; }

        public override string ToString()
        {
            return string.Format("SuiteName: {0}", SuiteName);
        }
    }
}