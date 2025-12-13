using System;
using TheChest.Core.Containers.Interfaces;
using TheChest.Inventories.Containers.Events.Stack.Lazy;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Stackable with lazy behavior
    /// </summary>
    /// <typeparam name="T">Item the Inventory accept</typeparam>
    public interface ILazyStackInventory<T> : IStackContainer<T>
    {
        /// <summary>
        /// Raised when an amount of item is requested from an index of the inventory
        /// </summary>
        event LazyStackInventoryGetEventHandler<T>? OnGet;
        /// <summary>
        /// Raised when an amount of item is added to an index of the inventory
        /// </summary>
        event LazyStackInventoryAddEventHandler<T>? OnAdd;
        /// <summary>
        /// Raised when one item is moved from an index to other on the inventory
        /// </summary>
        event LazyStackInventoryMoveEventHandler<T>? OnMove;

        #region ILazyStackInventory
        /// <summary>
        /// Gets an item from inside a slot
        /// </summary>
        /// <param name="index">Slot's inventory to be searched</param>
        /// <returns>Returns the item inside <paramref name="index"/> Slot</returns>
        T Get(int index);
        /// <summary>
        /// Search an Item from inventory
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>Returns the first item found or null</returns>
        T Get(T item);
        /// <summary>
        /// Search an amount of items in the inventory
        /// </summary>
        /// <param name="item">Item to be founded</param>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>Returns the amount of items searched (or the max it can)</returns>
        T[] Get(T item, int amount);
        /// <summary>
        /// Get all Item of the selected type from all slots
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>Returns a list with all items founded in the inventory</returns>
        T[] GetAll(T item);
        /// <summary>
        /// Returns the amount of an item inside the inventory
        /// </summary>
        /// <param name="item">The item to de counted</param>
        /// <returns>Returns the current amount of the item in the Inventory</returns>
        int GetCount(T item);
        /// <summary>
        /// Adds an item in a avaliable slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns>returns true if the item could be added</returns>
        bool Add(T item);
        #endregion

        #region IInventory
        /// <summary>
        /// Returns an amount of items inside the Inventory Slot
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <param name="amount">Amount of the item to be returned</param>
        /// <returns>Returns the amount of items inside the slot (or the max it can)</returns>
        T[] Get(int index, int amount);
        /// <summary>
        /// Returns all the items from the selected slot 
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <returns>An array with of items</returns>
        T[] GetAll(int index);
        /// <summary>
        /// Adds items inside the inventory
        /// </summary>
        /// <param name="item">Array of item of the same type wich will be added to inventory</param>
        /// <param name="amount">Amount of the item to be returned</param>
        /// <returns>Returns the amount of items that couldn't be added</returns>
        int Add(T item, int amount);
        /// <summary>
        /// Adds an amount of item in a specific slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <param name="index">slot where the item will be added</param>
        /// <param name="amount">amount of the item</param>
        /// <returns>Returns the amount of items that couldn't be added</returns>
        int AddAt(T item, int index, int amount);
        /// <summary>
        /// Adds or replace an amount of item in a specific slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <param name="index">slot where the item will be added</param>
        /// <param name="amount">amount of the item</param>
        /// <param name="replace">if true it will repleace the current items inside it</param>
        /// <returns>Returns the items that couldn't be added or the replaced</returns>
        [Obsolete("This method will be removed in the future versions. Use AddAt(T item, int index, int amount) instead")]
        T[] AddAt(T item, int index, int amount, bool replace);
        #endregion

        #region IInteractive
        /// <summary>
        /// Move a item between two slots
        /// </summary>
        /// <param name="origin">Selected item</param>
        /// <param name="target">Where the item will be placed</param>
        void Move(int origin, int target);
        /// <summary>
        /// Returns every item from the inventory
        /// </summary>
        /// <returns>Returns an Array of items</returns>
        T[] Clear();
        #endregion
    }
}
