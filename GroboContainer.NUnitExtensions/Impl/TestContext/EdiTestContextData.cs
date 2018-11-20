using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using GroboContainer.Core;

using JetBrains.Annotations;

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
                throw new InvalidOperationException($"Item with the same name is already added: {itemName}");
        }

        public bool RemoveItem([NotNull] string itemName)
        {
            return items.TryRemove(itemName, out _);
        }

        public bool TryDestroy(out AggregateException aggregateError)
        {
            var errors = new List<InvalidOperationException>();
            foreach (var kvp in items.OrderByDescending(x => x.Value.Order))
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

        private static bool TryDisposeItem([NotNull] string itemName, [NotNull] IDisposable disposableItem, out InvalidOperationException error)
        {
            error = null;
            try
            {
                disposableItem.Dispose();
                return true;
            }
            catch (Exception e)
            {
                error = new InvalidOperationException($"Failed to dispose item: {itemName}", e);
                return false;
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, items.Select(kvp => $"{kvp.Key}: ({kvp.Value.Order}, {kvp.Value.ItemValue})"));
        }

        private readonly Lazy<IContainer> lazyContainer;
        private readonly ConcurrentDictionary<string, ItemValueHolder> items;

        private class ItemValueHolder
        {
            public ItemValueHolder([NotNull] object itemValue)
            {
                Order = Interlocked.Increment(ref order);
                ItemValue = itemValue;
            }

            public int Order { get; }

            [NotNull]
            public object ItemValue { get; }

            private static int order;
        }
    }
}