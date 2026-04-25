using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Slots.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Slot
    /// </summary>
    /// <typeparam name="T">Item the Slot Accept</typeparam>
    public interface IInventorySlot<T> : ISlot<T>
    {
        /// <summary>
        /// Checks whether the specified item can be added to the collection.
        /// </summary>
        /// <param name="item">The item to be evaluated.</param>
        /// <returns>true if the item can be added; otherwise, false.</returns>
        bool CanAdd(T item);
        /// <summary>
        /// Adds the item in the current Slot if <see cref="ISlot{T}.IsFull"/> is false
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns>True if the value is successful added</returns>
        bool Add(T item);

        /// <summary>
        /// Checks whether the specified item can replace the content of the slot.
        /// </summary>
        /// <param name="item">The item to be evaluated.</param>
        /// <returns>true if the item can be replaced; otherwise, false.</returns>
        bool CanReplace(T item);
        /// <summary>
        /// Replaces the content of slot to item
        /// </summary>
        /// <param name="item">Item to replace</param>
        /// <returns>Old value of the slot</returns>
        T Replace(T item);

        /// <summary>
        /// Returns an item from slot
        /// </summary>
        /// <returns>Returns an item of the slot</returns>
        T Get();
    }
}