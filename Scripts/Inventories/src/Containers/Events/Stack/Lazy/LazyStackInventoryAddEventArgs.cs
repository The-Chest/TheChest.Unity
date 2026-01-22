using System;
using System.Collections.Generic;

namespace TheChest.Inventories.Containers.Events.Stack.Lazy
{
    /// <summary>
    /// Data for the Event args on <see cref="LazyStackInventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct LazyStackInventoryAddItemEventData<T>
    {
        /// <summary>
        /// Item that was added to the inventory.
        /// </summary>
        public T Item { get; }
        /// <summary>
        /// Index of the item in the inventory where it was added.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Amount of items that were added to the inventory.
        /// </summary>
        public int Amount { get; }
        /// <summary>
        /// Creates a new instance of <see cref="LazyStackInventoryAddItemEventData{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        public LazyStackInventoryAddItemEventData(T item, int index, int amount)
        {
            this.Item = item;
            this.Index = index;
            this.Amount = amount;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="LazyStackInventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LazyStackInventoryAddEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data that were sent throught the event.
        /// </summary>
        public IReadOnlyCollection<LazyStackInventoryAddItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates a new instance of <see cref="LazyStackInventoryAddEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public LazyStackInventoryAddEventArgs(IReadOnlyCollection<LazyStackInventoryAddItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a single item data to <see cref="LazyStackInventoryAddEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator LazyStackInventoryAddEventArgs<T>((T Item, int Index, int Amount) data)
        {
            return new LazyStackInventoryAddEventArgs<T>(
                new LazyStackInventoryAddItemEventData<T>[1] {
                    new LazyStackInventoryAddItemEventData<T>(
                        item: data.Item,
                        index: data.Index,
                        amount: data.Amount
                    )
                }
            );
        }
    }
    
    /// <summary>
    /// Event handler triggered when one or more items of the same type are added to an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void LazyStackInventoryAddEventHandler<T>(object? sender, LazyStackInventoryAddEventArgs<T> e);
}
