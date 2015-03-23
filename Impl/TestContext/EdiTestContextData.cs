using System.Collections.Generic;

using DiadocSys.Core.Json;

using GroboContainer.Core;
using GroboContainer.Impl;

using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;
using SKBKontur.Catalogue.ServiceLib;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public abstract class EdiTestContextData
    {
        protected EdiTestContextData()
        {
            Items = new Dictionary<string, object>();
        }

        [NotNull]
        public Dictionary<string, object> Items { get; private set; }

        [NotNull]
        public IContainer GetContainer()
        {
            object container;
            if(!Items.TryGetValue(ContainerItemKey, out container))
                throw new InvalidProgramStateException("Container is not set");
            return (IContainer)container;
        }

        public void InitContainer()
        {
            if(Items.ContainsKey(ContainerItemKey))
                throw new InvalidProgramStateException("Container is already created");
            Items[ContainerItemKey] = new Container(new ContainerConfiguration(AssembliesLoader.Load()));
        }

        public void DestroyContainer()
        {
            object container;
            if(Items.TryGetValue(ContainerItemKey, out container))
            {
                ((IContainer)container).Dispose();
                Items.Remove(ContainerItemKey);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Items.ToPrettyJson());
        }

        public const string ContainerItemKey = "__Container";
    }
}