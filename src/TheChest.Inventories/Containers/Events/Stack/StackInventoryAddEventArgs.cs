using System;
using System.Collections.Generic;

namespace TheChest.Inventories.Containers.Events.Stack
{
    /// <summary>
    /// Data for the Event args on <see cref="StackInventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct StackInventoryAddItemEventData<T>
    {
        /// <summary>
        /// Items that were added to the inventory.
        /// </summary>
        public T[] Items { get; }
        /// <summary>
        /// Index where the items were added.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Creates data for the <see cref="StackInventoryAddEventHandler{T}"/> event.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="index"></param>
        public StackInventoryAddItemEventData(T[] items, int index)
        {
            Items = items;
            Index = index;
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="StackInventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class StackInventoryAddEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<StackInventoryAddItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="StackInventoryAddEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public StackInventoryAddEventArgs(IReadOnlyCollection<StackInventoryAddItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a tuple to <see cref="StackInventoryAddEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator StackInventoryAddEventArgs<T>((T[] Items, int Index) data)
        {
            return new StackInventoryAddEventArgs<T>(
                new StackInventoryAddItemEventData<T>[1] {
                    new StackInventoryAddItemEventData<T>(
                        items: data.Items,
                        index: data.Index
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
    public delegate void StackInventoryAddEventHandler<T>(object? sender, StackInventoryAddEventArgs<T> e);
}
