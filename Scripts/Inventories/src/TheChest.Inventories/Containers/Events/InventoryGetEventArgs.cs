using System;
using System.Collections.Generic;
using System.Linq;

namespace TheChest.Inventories.Containers.Events
{
    /// <summary>
    /// Data for the Event args on <see cref="InventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct InventoryGetItemEventData<T>
    {
        /// <summary>
        /// Item that was requested from the inventory.
        /// </summary>
        public T Item { get; }
        /// <summary>
        /// Index from which the item was requested in the inventory.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Creates data for the <see cref="InventoryGetEventHandler{T}"/> event.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        public InventoryGetItemEventData(T item, int index)
        {
            Item = item;
            Index = index;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="InventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public sealed class InventoryGetEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<InventoryGetItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="InventoryGetEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public InventoryGetEventArgs(IReadOnlyCollection<InventoryGetItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a tuple to <see cref="InventoryGetEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator InventoryGetEventArgs<T>((T Item, int Index) data)
        {
            return new InventoryGetEventArgs<T>(
                new InventoryGetItemEventData<T>[] {
                    new InventoryGetItemEventData<T>(
                        item: data.Item, 
                        index: data.Index
                    )
                }
            );
        }
        /// <summary>
        /// Implicit conversion from an array of tuples to <see cref="InventoryGetEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator InventoryGetEventArgs<T>((T[] Items, int[] Indexes) data)
        {
            return new InventoryGetEventArgs<T>(
                data.Items.Select(
                (item, i) =>
                    new InventoryGetItemEventData<T>(
                        item: item,
                        index: data.Indexes[i]
                    )
                ).ToArray()
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is requested from an inventory. 
    /// <para>
    /// This event contract is the equivalent of <see cref="EventHandler{TEventArgs}"/> with <seealso cref="InventoryGetEventArgs{T}"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were requested</param>
    public delegate void InventoryGetEventHandler<T>(object? sender, InventoryGetEventArgs<T> e);
}
