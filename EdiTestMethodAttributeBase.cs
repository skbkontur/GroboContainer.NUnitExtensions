using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    [MeansImplicitUse]
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class EdiTestMethodAttributeBase : Attribute
    {
    }
}