using System;
using TheChest.Inventories.Containers.Events;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Inventory Interface with add and get methods
    /// </summary>
    /// <remarks>
    /// This interface is still unstable. Some methods can be moved to separated interfaces.
    /// </remarks>
    /// <typeparam name="T">An item type</typeparam>
    public interface IInventory<T> : IInteractiveContainer<T>
    {
        /// <summary>
        /// Raised when an amount of item is requested from an index of the inventory
        /// </summary>
        event InventoryGetEventHandler<T>? OnGet;
        /// <summary>
        /// Raised when an amount of item is added to an index of the inventory
        /// </summary>
        event InventoryAddEventHandler<T>? OnAdd;

        /// <summary>
        /// Gets an item inside a slot
        /// </summary>
        /// <param name="index">Slot's inventory to be searched</param>
        /// <returns>An item inside of the <paramref name="index"/> Slot</returns>
        T Get(int index);
        /// <summary>
        /// Search an item from inventory
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>First item found that is equal <paramref name="item"/></returns>
        T Get(T item);
        /// <summary>
        /// Search an amount of items in the inventory
        /// </summary>
        /// <param name="item">Item to be found</param>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>An array with the <paramref name="amount"/> of items equal to <paramref name="item"/> (or the max it can)</returns>
        T[] Get(T item, int amount);
        /// <summary>
        /// Get all Item of the selected type from all slots
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>An array with all items that are equal to <paramref name="item"/> in the inventory</returns>
        T[] GetAll(T item);
        /// <summary>
        /// Get the amount of an <paramref name="item"/> inside the inventory
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>The current amount of the <paramref name="item"/> in the Inventory</returns>
        int GetCount(T item);

        /// <summary>
        /// Adds and array of items in a avaliable slot
        /// </summary>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <returns>The items from param that were not possible to add</returns>
        T[] Add(T[] items);
        /// <summary>
        /// Adds an item in a avaliable slot 
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns>true if the <paramref name="item"/> could be added</returns>
        bool Add(T item);
        /// <summary>
        /// Adds an item in a specific slot
        /// <para>This method will be removed in the future versions. Use <see cref="AddAt(T, int)"/> instead.</para>
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <param name="index">Slot where the item will be added</param>
        /// <param name="replace">Flag that decide if the item on <paramref name="index"/> (if exists) will be replaced</param>
        /// <returns>The item from param that couldn't be added or the replaced item that were inside the slot</returns>
        [Obsolete("This method will be removed in the future versions. Use AddAt(T item, int index) instead")]
        T AddAt(T item, int index, bool replace);
        /// <summary>
        /// Adds an item in a specific slot
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <param name="index">Slot where the item will be added</param>
        /// <returns>true if the <paramref name="item"/> could be added to the <paramref name="index"/></returns>
        bool AddAt(T item, int index);
    }
}
