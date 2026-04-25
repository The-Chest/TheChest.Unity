using System;
using System.Collections.Generic;

namespace TheChest.Inventories.Containers.Events.Stack
{
    /// <summary>
    /// Data for an event that occurs when an item in an inventory is replaced.
    /// </summary>
    /// <typeparam name="T">The type of the items in the inventory.</typeparam>
    public readonly struct StackInventoryReplaceEventData<T>
    {
        /// <summary>
        /// Index of the current item.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Old items that were replaced.
        /// </summary>
        public T[] OldItems { get; }
        /// <summary>
        /// New items that replaced the old ones.
        /// </summary>
        public T[] NewItems { get; }
        /// <summary>
        /// Creates data for an event that occurs when an item in an inventory is replaced.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldItems"></param>
        /// <param name="newItems"></param>
        public StackInventoryReplaceEventData(int index, T[] oldItems, T[] newItems)
        {
            Index = index;
            OldItems = oldItems;
            NewItems = newItems;
        }
    }

    /// <summary>
    /// Data for an event that occurs when items in an inventory are replaced.
    /// </summary>
    /// <typeparam name="T">The type of the items in the inventory.</typeparam>
    public sealed class StackInventoryReplaceEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the collection of inventory replacement event data.
        /// </summary>
        public IReadOnlyCollection<StackInventoryReplaceEventData<T>> Data { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="StackInventoryReplaceEventArgs{T}"/> class.
        /// </summary>
        /// <param name="data">A collection of <see cref="StackInventoryReplaceEventData{T}"/></param>
        public StackInventoryReplaceEventArgs(IReadOnlyCollection<StackInventoryReplaceEventData<T>> data)
        {
            Data = data;
        }

        /// <summary>
        /// Provide implicit conversion from a tuple to StackInventoryReplaceEventArgs.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator StackInventoryReplaceEventArgs<T>((int Index, T[] OldItems, T[] NewItems) data)
        {
            return new StackInventoryReplaceEventArgs<T>(
                new StackInventoryReplaceEventData<T>[] {
                    new StackInventoryReplaceEventData<T>(
                        oldItems: data.OldItems,
                        newItems: data.NewItems,
                        index: data.Index
                    )
                }
            );
        }
    }

    /// <summary>
    /// Event handler triggered when a items are replaced from an index.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void StackInventoryReplaceEventHandler<T>(object? sender, StackInventoryReplaceEventArgs<T> e);
}
