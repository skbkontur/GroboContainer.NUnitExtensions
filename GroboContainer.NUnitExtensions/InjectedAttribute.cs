using System;
using System.Diagnostics.CodeAnalysis;

namespace GroboContainer.NUnitExtensions
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class InjectedAttribute : Attribute
    {
    }
}