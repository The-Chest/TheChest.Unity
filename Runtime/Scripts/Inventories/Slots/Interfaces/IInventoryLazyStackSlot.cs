using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Slots.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Stackable Slot with lazy behavior
    /// </summary>
    /// <para>
    /// This interface is still unstable. Some methods can be moved to a separated interface.
    /// </para>
    /// <typeparam name="T">Item the Slot Accept</typeparam>
    public interface IInventoryLazyStackSlot<T> : ILazyStackSlot<T>
    {
        /// <summary>
        /// Gets the current available amount.
        /// </summary>
        /// <remarks>This property will be moved to <see cref="ILazyStackSlot{T}"/></remarks>
        int AvailableAmount { get; }
        /// <summary>
        /// Checks if the slot can add an amount of items
        /// </summary>
        /// <param name="item">item to be added to the slot</param>
        /// <param name="amount">the amount of the <paramref name="item"/> to be added to the slot</param>
        /// <returns>true if is possible to add the amount of the <paramref name="item"/> inside the slot</returns>
        bool CanAdd(T item, int amount = 1);
        /// <summary>
        /// Add an amount of items inside the current slot
        /// </summary>
        /// <param name="item">The item to be added </param>
        /// <param name="amount">The amount of items added</param>
        /// <returns>Return 0 if all items are fully added to slot, else will return the amount left</returns>
        int Add(T item, int amount = 1);
        /// <summary>
        /// Checks if the slot can replace an item
        /// </summary>
        /// <param name="item">The item to be added and replace the old ones</param>
        /// <param name="amount">The amount of items that will replace</param>
        /// <returns>true if is possible to replace</returns>
        bool CanReplace(T item, int amount = 1);
        /// <summary>
        /// Remove the current item of Slot and replace by a new one
        /// </summary>
        /// <param name="item">The item wich will replace the old one</param>
        /// <param name="amount">The amount of the New item</param>
        /// <returns>Returns an array of the old item</returns>
        T[] Replace(T item, int amount = 1);
        /// <summary>
        /// Gets an amount of items from the slot
        /// </summary>
        /// <param name="amount">Desired amount to be returned</param>
        /// <returns>an array items from the slot</returns>
        T[] Get(int amount = 1);
        /// <summary>
        /// Gets all items from inside the slot. 
        /// </summary>
        /// <returns>An array with all items from slot</returns>
        T[] GetAll();
    }
}
