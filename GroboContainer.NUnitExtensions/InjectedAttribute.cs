using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class InjectedAttribute : Attribute
    {
    }
}