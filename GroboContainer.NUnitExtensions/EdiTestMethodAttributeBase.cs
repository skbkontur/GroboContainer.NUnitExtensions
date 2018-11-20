using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    [MeansImplicitUse]
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class EdiTestMethodAttributeBase : Attribute
    {
    }
}