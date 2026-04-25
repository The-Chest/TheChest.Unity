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
        event InventoryGetEventHandler<T> OnGet;

        /// <summary>
        /// Gets an item inside a slot
        /// </summary>
        /// <param name="index">Slot's inventory to be searched</param>
        /// <returns>An item inside of the <paramref name="index"/> Slot</returns>
        T Get(int index);
        /// <summary>
        /// Gets an item inside the inventory
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>First item found that is equal <paramref name="item"/></returns>
        T Get(T item);
        /// <summary>
        /// Gets an amount of items in the inventory
        /// </summary>
        /// <param name="item">Item to be found</param>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>An array with the <paramref name="amount"/> of items equal to <paramref name="item"/></returns>
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
        /// Raised when an amount of item is added to an index
        /// </summary>
        event InventoryAddEventHandler<T> OnAdd;
        /// <summary>
        /// Checks if <paramref name="item"/> can be added to any slot.
        /// </summary>
        /// <param name="item">The item to evaluate for addition to the inventory.</param>
        /// <returns>true if the <paramref name="item"/> can be added; otherwise, false.</returns>
        bool CanAdd(T item);
        /// <summary>
        /// Checks if <paramref name="items"/> can be added to any slot.
        /// </summary>
        /// <param name="items">An array of items to evaluate for addition to the inventory.</param>
        /// <returns>true if ALL <paramref name="items"/> can be added; otherwise, false.</returns>
        bool CanAdd(params T[] items);
        /// <summary>
        /// Determines whether the specified item can be added at the given index.
        /// </summary>
        /// <param name="item">The item to evaluate for insertion at the specified index.</param>
        /// <param name="index">The zero-based index at which to check if the item can be added. Must be within the valid range of the
        /// collection.</param>
        /// <returns>true if the item can be added at the specified index; otherwise, false.</returns>
        bool CanAddAt(T item, int index);

        /// <summary>
        /// <para> Adds an item in a avaliable slot </para>
        /// <para> This method return will change to void in future versions. </para>
        /// <para> Use <see cref="CanAdd(T)"/> to check if the item can be added before calling this method.</para>
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns>true if the <paramref name="item"/> could be added</returns>
        bool Add(T item);
        /// <summary>
        /// Adds and array of items in a avaliable slot
        /// </summary>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <returns>The items from param that were not possible to add</returns>
        T[] Add(params T[] items);
        /// <summary>
        /// <para>Adds an item in a specific slot </para>
        /// <para>This method return will change to void in future versions.</para>
        /// <para>Use <see cref="CanAddAt(T,int)"/> to check if the item can be added before calling this method.</para>
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <param name="index">Slot where the item will be added</param>
        /// <returns>true if the <paramref name="item"/> could be added to the <paramref name="index"/></returns>
        bool AddAt(T item, int index);

        /// <summary>
        /// Raised when an item is replaced in a specific slot
        /// </summary>
        event InventoryReplaceEventHandler<T> OnReplace;
        /// <summary>
        /// Checks if an item can be replaced in a specific slot
        /// </summary>
        /// <param name="item">Item to be checked to ocupy the slot on <paramref name="index"/></param>
        /// <param name="index">The index of the slot to be checked for replacement</param>
        /// <returns>true if the <paramref name="item"/> can be replaced in the <paramref name="index"/> slot; otherwise, false.</returns>
        bool CanReplace(T item, int index);
        /// <summary>
        /// Replaces an item in a specific slot
        /// </summary>
        /// <param name="item">The item that will now ocupy the slot on <paramref name="index"/></param>
        /// <param name="index">The index of the <paramref name="item"/> will ocupy</param>
        /// <returns>The old item from the slot on <paramref name="index"/></returns>
        T Replace(T item, int index);

        /// <summary>
        /// Raised when one item is moved from an index to other on the inventory
        /// </summary>
        event InventoryMoveEventHandler<T> OnMove;
    }
}
