using TheChest.Core.Containers.Interfaces;
using TheChest.Inventories.Containers.Events.Stack;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Interface with methods for interaction with the Inventory using stacks
    /// <para>
    /// This interface is still unstable. Some methods can be moved to a separated interface.
    /// </para>
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public interface IStackInventory<T> : IStackContainer<T>
    {
        /// <summary>
        /// Raised when an amount of item is added to an index of the inventory
        /// </summary>
        event StackInventoryAddEventHandler<T> OnAdd;
        /// <summary>
        /// Raised when an amount of item is requested from an index of the inventory
        /// </summary>
        event StackInventoryGetEventHandler<T> OnGet;
        /// <summary>
        /// Raised when one item is moved from an index to other on the inventory
        /// </summary>
        event StackInventoryMoveEventHandler<T> OnMove;
        /// <summary>
        /// Raised when an item is removed from an index of the inventory
        /// </summary>
        event StackInventoryReplaceEventHandler<T> OnReplace;

        #region IStackInventory
        /// <summary>
        /// Gets an item from inside a slot index
        /// </summary>
        /// <param name="index">Slot's inventory to be searched</param>
        /// <returns>Returns the item inside <paramref name="index"/> Slot</returns>
        T Get(int index);
        /// <summary>
        /// Search an Item from inventory
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>Returns the first item found</returns>
        T Get(T item);
        /// <summary>
        /// Search an amount of items in the inventory
        /// </summary>
        /// <param name="item">Item to be found</param>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>An array with amount of items searched (or the max it can)</returns>
        T[] Get(T item, int amount);
        /// <summary>
        /// Get all Items of the selected type from all slots
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>An array with all items found in the inventory</returns>
        T[] GetAll(T item);
        /// <summary>
        /// Gets the amount of an item inside the inventory
        /// </summary>
        /// <param name="item">The item to de counted</param>
        /// <returns>The current amount of the item in the Inventory</returns>
        int GetCount(T item);

        /// <summary>
        /// Checks if <paramref name="item"/> can be added to any slot on inventory.
        /// </summary>
        /// <param name="item">The item to evaluate to add to the inventory.</param>
        /// <returns>true if the <paramref name="item"/> can be added; otherwise, false.</returns>
        bool CanAdd(T item);
        /// <summary>
        /// Checks if <paramref name="items"/> can be added to any slot on inventory.
        /// </summary>
        /// <param name="items">An array of items to evaluate for addition to the inventory.</param>
        /// <returns>true if ALL <paramref name="items"/> can be added; otherwise, false.</returns>
        bool CanAdd(params T[] items);

        /// <summary>
        /// Adds and array of item in a avaliable slot
        /// </summary>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <returns></returns>
        T[] Add(params T[] items);
        /// <summary>
        /// Adds an item in a avaliable slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns>true if is possible to add <paramref name="item"/></returns>
        bool Add(T item);

        /// <summary>
        /// Checks if it's possible to replace the items in a specific slot with the given items
        /// </summary>
        /// <param name="items">The items that will check to be replaced the slot on <paramref name="index"/></param>
        /// <param name="index">The index of the slot to be checked if can be replaced by <paramref name="items"/></param>
        /// <returns>True if the <paramref name="items"/> can replace the slot on <paramref name="index"/>; otherwise, false.</returns>
        bool CanReplace(T[] items, int index);
        /// <summary>
        /// Replaces items in a specific slot
        /// </summary>
        /// <param name="items">The items that will now ocupy the slot on <paramref name="index"/></param>
        /// <param name="index">The index of the <paramref name="items"/> will ocupy</param>
        /// <returns>The old items from the slot on <paramref name="index"/></returns>
        T[] Replace(T[] items, int index);
        #endregion

        #region IInventory
        /// <summary>
        /// Gets an amount of items inside the Inventory Slot
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <param name="amount">Amount of the item to be returned</param>
        /// <returns>An array with the amount of items inside the slot (or the max it can)</returns>
        T[] Get(int index, int amount);
        /// <summary>
        /// Returns all the items from the selected slot 
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <returns>An array with of items</returns>
        T[] GetAll(int index);

        /// <summary>
        /// Determines whether the specified item can be added at the given index.
        /// </summary>
        /// <param name="item">The item to evaluate for insertion at the specified index.</param>
        /// <param name="index">The zero-based index at which to check if the item can be added.</param>
        /// <returns>true if the item can be added at the specified index; otherwise, false.</returns>
        bool CanAddAt(T item, int index);
        /// <summary>
        /// Determines whether the specified items can be added at the given index.
        /// </summary>
        /// <param name="items">The array of items to evaluate for insertion</param>
        /// <param name="index">The zero-based index at which to check if the items can be added.</param>
        /// <returns>true if all of the items can be added at the specified index; otherwise, false.</returns>
        bool CanAddAt(T[] items, int index);

        /// <summary>
        /// Adds an item in a specific slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <param name="index">slot where the item will be added</param>
        /// <returns>True if the <paramref name="item"/> could be added to the <paramref name="index"/></returns>
        bool AddAt(T item, int index);
        /// <summary>
        /// Adds an array of items inside the inventory
        /// </summary>
        /// <param name="items">Array of item of the same type wich will be added to inventory</param>
        /// <param name="index">Wich slot the items will be added</param>
        /// <returns>An array of items or couldn't be added to the <paramref name="index"/></returns>
        T[] AddAt(T[] items, int index);
        #endregion

        #region IInteractiveContainer
        /// <summary>
        /// Checks if the specified items can be moved from the origin index to the target index.
        /// </summary>
        /// <param name="origin">The zero-based index representing the items current position.</param>
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
        #endregion
    }
}
