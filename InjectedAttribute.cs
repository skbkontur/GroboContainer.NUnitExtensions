using System;
using System.Diagnostics.CodeAnalysis;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class InjectedAttribute : Attribute
    {
        public InjectedAttribute()
        {
        }

        public InjectedAttribute(ContainerMode containerMode)
        {
            ContainerMode = containerMode;
        }

        public ContainerMode ContainerMode { get; private set; }
    }

    public enum ContainerMode
    {
        Get,
        Create
    }
}