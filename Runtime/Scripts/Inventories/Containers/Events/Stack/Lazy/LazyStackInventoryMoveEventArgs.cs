using System;
using System.Collections.Generic;

namespace TheChest.Inventories.Containers.Events.Stack.Lazy
{
    /// <summary>
    /// Data for the Event args on <see cref="LazyStackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct LazyStackInventoryMoveItemEventData<T>
    {
        /// <summary>
        /// Item that was moved in the inventory.
        /// </summary>
        public T Item { get; }
        /// <summary>
        /// Amount of items that were moved in the inventory.
        /// </summary>
        public int Amount { get; }
        /// <summary>
        /// Index of the item in the inventory before the move operation.
        /// </summary>
        public int FromIndex { get; }
        /// <summary>
        /// Index of the item in the inventory after the move operation.
        /// </summary>
        public int ToIndex { get; }
        /// <summary>
        /// Creates data for the <see cref="LazyStackInventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        public LazyStackInventoryMoveItemEventData(T item, int amount, int fromIndex, int toIndex)
        {
            Item = item;
            Amount = amount;
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="LazyStackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LazyStackInventoryMoveEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<LazyStackInventoryMoveItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="LazyStackInventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public LazyStackInventoryMoveEventArgs(IReadOnlyCollection<LazyStackInventoryMoveItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a tuple to <see cref="LazyStackInventoryMoveEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator LazyStackInventoryMoveEventArgs<T>(
            (T Item, int Amount, int OriginIndex, int TargetIndex) data
        )
        {
            return new LazyStackInventoryMoveEventArgs<T>(
                new LazyStackInventoryMoveItemEventData<T>[1]
                {
                    new LazyStackInventoryMoveItemEventData<T>(data.Item, data.Amount, data.OriginIndex, data.TargetIndex),
                }
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is moved in an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void LazyStackInventoryMoveEventHandler<T>(object? sender, LazyStackInventoryMoveEventArgs<T> e);
}
