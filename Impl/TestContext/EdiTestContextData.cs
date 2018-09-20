using System;
using System.Collections.Concurrent;
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
            items = new ConcurrentDictionary<string, ItemValueHolder>();
        }

        [NotNull]
        public IContainer Container => lazyContainer.Value;

        public bool TryGetContextItem([NotNull] string itemName, out object itemValue)
        {
            itemValue = null;
            var result = items.TryGetValue(itemName, out var holder);
            if (result)
                itemValue = holder.ItemValue;
            return result;
        }

        public void AddItem([NotNull] string itemName, [NotNull] object itemValue)
        {
            if (!items.TryAdd(itemName, new ItemValueHolder(itemValue)))
                throw new InvalidProgramStateException($"Item with the same name is already added: {itemName}");
        }

        public bool RemoveItem([NotNull] string itemName)
        {
            return items.TryRemove(itemName, out _);
        }

        public bool TryDestroy(out AggregateException aggregateError)
        {
            var errors = new List<InvalidProgramStateException>();
            foreach (var kvp in items.OrderByDescending(x => x.Value.Timestamp))
            {
                if (kvp.Value.ItemValue is IDisposable disposableItem)
                {
                    if (!TryDisposeItem(kvp.Key, disposableItem, out var error))
                        errors.Add(error);
                }
            }
            items.Clear();
            if (errors.Any())
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
            catch (Exception e)
            {
                error = new InvalidProgramStateException($"Failed to dispose item: {itemName}", e);
                return false;
            }
        }

        public override string ToString()
        {
            return $"{items.ToPrettyJson()}";
        }

        private readonly Lazy<IContainer> lazyContainer;
        private readonly ConcurrentDictionary<string, ItemValueHolder> items;

        private class ItemValueHolder
        {
            public ItemValueHolder([NotNull] object itemValue)
            {
                Timestamp = Timestamp.Now;
                ItemValue = itemValue;
            }

            public Timestamp Timestamp { get; }

            [NotNull]
            public object ItemValue { get; }
        }
    }
}