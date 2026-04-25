using System;
using System.Collections.Generic;

namespace TheChest.Inventories.Containers.Events.Stack.Lazy
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct LazyStackInventoryReplaceItemEventData<T>
    {
        /// <summary>
        /// Index of the current items.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Old item that was replaced.
        /// </summary>
        public T OldItem { get; }
        /// <summary>
        /// The amount of old items that were replaced.
        /// </summary>
        public int OldAmount { get; }
        /// <summary>
        /// New item that replaced the old one.
        /// </summary>
        public T NewItem { get; }
        /// <summary>
        /// The amount of the items that replaced the old ones.
        /// </summary>
        public int NewAmount { get; }
        /// <summary>
        /// Creates data for an event that occurs when an item in an inventory is replaced.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newItem"></param>
        /// <param name="newAmount"></param>
        /// <param name="oldItem"></param>
        /// <param name="oldAmount"></param>
        public LazyStackInventoryReplaceItemEventData(T newItem, int newAmount, T oldItem, int oldAmount, int index)
        {
            NewItem = newItem;
            NewAmount = newAmount;
            OldItem = oldItem;
            OldAmount = oldAmount;
            Index = index;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="LazyStackInventoryReplaceEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LazyStackInventoryReplaceEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<LazyStackInventoryReplaceItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="LazyStackInventoryReplaceEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public LazyStackInventoryReplaceEventArgs(IReadOnlyCollection<LazyStackInventoryReplaceItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a tuple to <see cref="LazyStackInventoryReplaceEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator LazyStackInventoryReplaceEventArgs<T>(
            (T OldItem, int OldAmount, T NewItem, int NewAmount, int Index) data
        )
        {
            return new LazyStackInventoryReplaceEventArgs<T>(
                new LazyStackInventoryReplaceItemEventData<T>[1]
                {
                    new LazyStackInventoryReplaceItemEventData<T>(
                        data.NewItem, data.NewAmount,
                        data.OldItem, data.OldAmount,
                        data.Index
                    ),
                }
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is replaced in an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void LazyStackInventoryReplaceEventHandler<T>(object? sender, LazyStackInventoryReplaceEventArgs<T> e);
}
