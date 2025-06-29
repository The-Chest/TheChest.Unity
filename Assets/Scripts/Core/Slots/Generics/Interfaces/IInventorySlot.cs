namespace TheChest.Slots.Generics.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Slot
    /// </summary>
    /// <typeparam name="T">Item the Slot Accept</typeparam>
    public interface IInventorySlot<T> : ISlot<T>
    {
        /// <summary>
        /// Add the item in the current Slot
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns>True if the value is successful added</returns>
        bool Add(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        T Replace(T item);

        /// <summary>
        /// Returns an item from slot
        /// </summary>
        /// <returns>Returns an item of the slot, if <see cref="ISlot{T}.IsEmpty"/> returns null</returns>
        T GetOne();
    }
}
