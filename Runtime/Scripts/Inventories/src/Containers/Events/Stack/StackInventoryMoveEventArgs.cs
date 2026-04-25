using System;
using System.Collections.Generic;

namespace TheChest.Inventories.Containers.Events.Stack
{
    /// <summary>
    /// Data for the Event args on <see cref="StackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct StackInventoryMoveItemEventData<T>
    {
        /// <summary>
        /// Items that were moved in the inventory.
        /// </summary>
        public T[] Items { get; }
        /// <summary>
        /// Index of the items in the inventory before the move operation.
        /// </summary>
        public int FromIndex { get; }
        /// <summary>
        /// Index of the items in the inventory after the move operation.
        /// </summary>
        public int ToIndex { get; }
        /// <summary>
        /// Creates data for the <see cref="StackInventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        public StackInventoryMoveItemEventData(T[] items, int fromIndex, int toIndex)
        {
            this.Items = items;
            this.FromIndex = fromIndex;
            this.ToIndex = toIndex;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="StackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class StackInventoryMoveEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<StackInventoryMoveItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="StackInventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public StackInventoryMoveEventArgs(IReadOnlyCollection<StackInventoryMoveItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a tuple to <see cref="StackInventoryMoveEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator StackInventoryMoveEventArgs<T>(
            (T[] Items, int OriginIndex, int TargetIndex) data
        )
        {
            return new StackInventoryMoveEventArgs<T>(
                new StackInventoryMoveItemEventData<T>[1]
                {
                    new StackInventoryMoveItemEventData<T>(data.Items, data.OriginIndex, data.TargetIndex),
                }
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is added to an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void StackInventoryMoveEventHandler<T>(object? sender, StackInventoryMoveEventArgs<T> e);
}
