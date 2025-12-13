using System;
using System.Collections.Generic;
using System.Linq;

namespace TheChest.Inventories.Containers.Events
{
    /// <summary>
    /// Data for the Event args on <see cref="InventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct InventoryMoveItemEventData<T>
    {
        /// <summary>
        /// Item that was moved in the inventory.
        /// </summary>
        public T Item { get; }
        /// <summary>
        /// Index from which the item was moved in the inventory.
        /// </summary>
        public int FromIndex { get; }
        /// <summary>
        /// Index to which the item was moved in the inventory.
        /// </summary>
        public int ToIndex { get; }
        /// <summary>
        /// Creates data for the <see cref="InventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        public InventoryMoveItemEventData(T item, int fromIndex, int toIndex)
        {
            Item = item;
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="InventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InventoryMoveEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<InventoryMoveItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="InventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public InventoryMoveEventArgs(IReadOnlyCollection<InventoryMoveItemEventData<T>> data)
        {
            Data = data;
        }

        /// <summary>
        /// Implicit conversion from a tuple to <see cref="InventoryMoveEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator InventoryMoveEventArgs<T>(
            (T Item, int OriginIndex, int TargetIndex) data
        )
        {
            return new InventoryMoveEventArgs<T>(
                new InventoryMoveItemEventData<T>[1]
                {
                    new InventoryMoveItemEventData<T>(                        
                        item: data.Item,
                        fromIndex: data.OriginIndex,
                        toIndex: data.TargetIndex
                     ),
                }
            );
        }

        /// <summary>
        /// Implicit conversion from an array of tuples to <see cref="InventoryMoveEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator InventoryMoveEventArgs<T>(
            (T Item, int OriginIndex, int TargetIndex)[] data
        )
        {
            return new InventoryMoveEventArgs<T>(
                data.Select(
                    item => new InventoryMoveItemEventData<T>(
                        item: item.Item,
                        fromIndex: item.OriginIndex,
                        toIndex: item.TargetIndex
                    )
                ).ToArray()
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is moved from one index to another in the inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void InventoryMoveEventHandler<T>(object? sender, InventoryMoveEventArgs<T> e);
}
