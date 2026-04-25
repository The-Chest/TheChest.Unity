using System;
using TheChest.Core.Containers.Interfaces;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Interface with methods for interaction with the Container 
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    [Obsolete("Do not inherit it directly, use IInventory<T> or IStackInventory<T> instead")]
    public interface IInteractiveContainer<T> : IContainer<T>
    {
        /// <summary>
        /// Checks if the specified item can be moved from the origin index to the target index.
        /// </summary>
        /// <param name="origin">The zero-based index representing the item's current position.</param>
        /// <param name="target">The zero-based index representing the desired target position.</param>
        /// <returns>true if the item can be moved to the target index; otherwise, false.</returns>
        bool CanMove(int origin, int target);
        /// <summary>
        /// Moves an item from one index to another in the inventory
        /// </summary>
        /// <param name="origin">Selected item</param>
        /// <param name="target">Where the item will be placed</param>
        void Move(int origin, int target);

        /// <summary>
        /// Gets every item from inventory
        /// </summary>
        /// <returns>Returns an Array of items</returns>
        T[] Clear();
    }
}
