using System;
using System.Collections.Generic;

namespace TheChest.Inventories.Containers.Events.Stack.Lazy
{
    /// <summary>
    /// Data for the Event args on <see cref="LazyStackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct LazyStackInventoryGetItemEventData<T>
    {
        /// <summary>
        /// Item that was retrieved from the inventory.
        /// </summary>
        public T Item { get; }
        /// <summary>
        /// Index of the item in the inventory from which it was retrieved.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Amount of items that were retrieved from the inventory.
        /// </summary>
        public int Amount { get; }
        /// <summary>
        /// Creates a new instance of <see cref="LazyStackInventoryGetItemEventData{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        public LazyStackInventoryGetItemEventData(T item, int index, int amount)
        {
            Item = item;
            Index = index;
            Amount = amount;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="LazyStackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Items type</typeparam>
    public sealed class LazyStackInventoryGetEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="LazyStackInventoryGetEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public LazyStackInventoryGetEventArgs(IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>> data)
        {
            Data = data;
        }

        /// <summary>
        /// Implicit conversion from an array of tuples to <see cref="LazyStackInventoryGetEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator LazyStackInventoryGetEventArgs<T>((T Item, int Index, int Amount) data)
        {
            return new LazyStackInventoryGetEventArgs<T>(
                new LazyStackInventoryGetItemEventData<T>[1] {
                    new LazyStackInventoryGetItemEventData<T>(
                        item: data.Item, 
                        index: data.Index, 
                        amount: data.Amount
                    )
                }
            );
        }
    }

    /// <summary>
    /// Event handler for the <see cref="LazyStackInventoryGetEventArgs{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LazyStackInventoryGetEventHandler<T>(object? sender, LazyStackInventoryGetEventArgs<T> e);
}
