using System;
using System.Collections.Generic;
using System.Linq;

using GroboContainer.Core;

using JetBrains.Annotations;

using SKBKontur.Catalogue.Objects;
using SKBKontur.Catalogue.Objects.Json;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext
{
    public abstract class EdiTestContextData : IEditableEdiTestContext
    {
        protected EdiTestContextData([NotNull] Lazy<IContainer> lazyContainer)
        {
            this.lazyContainer = lazyContainer;
            items = new Dictionary<string, ItemValueHolder>();
        }

        [NotNull]
        public IContainer Container { get { return lazyContainer.Value; } }

        public bool TryGetContextItem([NotNull] string itemName, out object itemValue)
        {
            itemValue = null;
            ItemValueHolder holder;
            var result = items.TryGetValue(itemName, out holder);
            if(result)
                itemValue = holder.ItemValue;
            return result;
        }

        public void AddItem([NotNull] string itemName, [NotNull] object itemValue)
        {
            if(items.ContainsKey(itemName))
                throw new InvalidProgramStateException(string.Format("Item with the same name is already added: {0}", itemName));
            items.Add(itemName, new ItemValueHolder(items.Count, itemValue));
        }

        public bool RemoveItem([NotNull] string itemName)
        {
            return items.Remove(itemName);
        }

        public bool TryDestroy(out AggregateException aggregateError)
        {
            var errors = new List<InvalidProgramStateException>();
            foreach(var kvp in items.OrderByDescending(x => x.Value.Order))
            {
                var disposableItem = kvp.Value.ItemValue as IDisposable;
                if(disposableItem != null)
                {
                    InvalidProgramStateException error;
                    if(!TryDisposeItem(kvp.Key, disposableItem, out error))
                        errors.Add(error);
                }
            }
            items.Clear();
            if(errors.Any())
            {
                aggregateError = new AggregateException("Failed to dispose at least one of context items", errors);
                return false;
            }
            aggregateError = null;
            return true;
        }

        private static bool TryDisposeItem([NotNull] string itemName, [NotNull] IDisposable disposableItem, out InvalidProgramStateException error)
        {
            error = null;
            try
            {
                disposableItem.Dispose();
                return true;
            }
            catch(Exception e)
            {
                error = new InvalidProgramStateException(string.Format("Failed to dispose item: {0}", itemName), e);
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", items.ToPrettyJson());
        }

        private readonly Lazy<IContainer> lazyContainer;
        private readonly Dictionary<string, ItemValueHolder> items;

        private class ItemValueHolder
        {
            public ItemValueHolder(int order, [NotNull] object itemValue)
            {
                Order = order;
                ItemValue = itemValue;
            }

            public int Order { get; private set; }

            [NotNull]
            public object ItemValue { get; private set; }
        }
    }
}