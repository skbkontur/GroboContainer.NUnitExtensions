using System;
using System.Diagnostics.CodeAnalysis;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EdiTestFixtureAttribute : EdiTestSuiteAttributeBase
    {
    }
}