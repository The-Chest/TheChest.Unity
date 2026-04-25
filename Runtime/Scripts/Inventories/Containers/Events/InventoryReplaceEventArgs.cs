using System;
using System.Collections.Generic;
using System.Linq;

namespace TheChest.Inventories.Containers.Events
{
    /// <summary>
    /// Data for an event that occurs when an item in an inventory is replaced.
    /// </summary>
    /// <typeparam name="T">The type of the items in the inventory.</typeparam>
    public readonly struct InventoryReplaceEventData<T>
    {
        /// <summary>
        /// Index of the current item.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Gets the previous item that was replaced in the collection.
        /// </summary>
        public T OldItem { get; }
        /// <summary>
        /// Gets the newly created item of type <typeparamref name="T"/>.
        /// </summary>
        public T NewItem { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryReplaceEventData{T}"/> class, representing an event
        /// where an item in an inventory is replaced.
        /// </summary>
        /// <param name="index">The index of the item being replaced in the inventory.</param>
        /// <param name="oldItem">The item that was previously at <paramref name="index"/>.</param>
        /// <param name="newItem">The new item that replaces <paramref name="oldItem"/> at <paramref name="index"/>.</param>
        public InventoryReplaceEventData(int index, T oldItem, T newItem)
        {
            this.Index = index;
            this.OldItem = oldItem;
            this.NewItem = newItem;
        }
    }

    /// <summary>
    /// Data for an event that occurs when items in an inventory are replaced.
    /// </summary>
    /// <typeparam name="T">The type of the items in the inventory.</typeparam>
    public sealed class InventoryReplaceEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the collection of inventory replacement event data.
        /// </summary>
        public IReadOnlyCollection<InventoryReplaceEventData<T>> Data { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryReplaceEventArgs{T}"/> class.
        /// </summary>
        /// <param name="data">A collection of <see cref="InventoryReplaceEventData{T}"/></param>
        public InventoryReplaceEventArgs(IReadOnlyCollection<InventoryReplaceEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Defines an implicit conversion from a tuple containing an index, old item, and new item  to an <see
        /// cref="InventoryReplaceEventArgs{T}"/> instance.
        /// </summary>
        /// <param name="data">A tuple containing the index of the replaced item, the old item being replaced, and the new item.</param>
        public static implicit operator InventoryReplaceEventArgs<T>((int Index, T OldItem, T NewItem) data)
        {
            return new InventoryReplaceEventArgs<T>(
                new InventoryReplaceEventData<T>[] {
                    new InventoryReplaceEventData<T>(
                        oldItem: data.OldItem,
                        newItem: data.NewItem,
                        index: data.Index
                    )
                }
            );
        }
        /// <summary>
        /// Defines an implicit conversion from a tuple containing arrays of indexes, old items, and new items
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator InventoryReplaceEventArgs<T>((int[] Indexes, T[] OldItems, T[] NewItems) data)
        {
            return new InventoryReplaceEventArgs<T>(
                data.Indexes.Select(
                (index, i) =>
                    new InventoryReplaceEventData<T>(
                        index: index,
                        newItem : data.NewItems[i],
                        oldItem : data.OldItems[i]
                    )
                ).ToArray()
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is replaced from an index.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void InventoryReplaceEventHandler<T>(object? sender, InventoryReplaceEventArgs<T> e);
}
