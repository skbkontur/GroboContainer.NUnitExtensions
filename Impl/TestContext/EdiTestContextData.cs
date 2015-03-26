using System;
using System.Collections.Generic;

using DiadocSys.Core.Json;

using GroboContainer.Core;
using GroboContainer.Impl;

using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;
using SKBKontur.Catalogue.ServiceLib;
using SKBKontur.Catalogue.ServiceLib.Logging;

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
                TryDisposeContainer((IContainer)container);
                Items.Remove(ContainerItemKey);
            }
        }

        private void TryDisposeContainer([NotNull] IContainer container)
        {
            try
            {
                container.Dispose();
            }
            catch(Exception e)
            {
                Log.For(this).Fatal("Failed to dispose container", e);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Items.ToPrettyJson());
        }

        public const string ContainerItemKey = "__Container";
    }
}