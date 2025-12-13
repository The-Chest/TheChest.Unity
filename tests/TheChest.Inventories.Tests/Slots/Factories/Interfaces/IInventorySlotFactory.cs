using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Factories.Interfaces
{
    /// <summary>
    /// Factory interface to instantiate any <see cref="IInventorySlot{T}"/>
    /// </summary>
    /// <typeparam name="T">Any type of item inside IInventorySlot</typeparam>
    public interface IInventorySlotFactory<T>
    {
        /// <summary>
        /// Creates an <see cref="IInventorySlot{T}"/> with no item inside it
        /// </summary>
        /// <returns>An empty <see cref="IInventorySlot{T}"/></returns>
        IInventorySlot<T> EmptySlot();
        /// <summary>
        /// Creates an <see cref="IInventorySlot{T}"/> with an item inside it
        /// </summary>
        /// <param name="item">The item that will be inside the created <see cref="IInventorySlot{T}"/></param>
        /// <returns>A full <see cref="IInventorySlot{T}"/></returns>
        IInventorySlot<T> FullSlot(T item);
    }
}
