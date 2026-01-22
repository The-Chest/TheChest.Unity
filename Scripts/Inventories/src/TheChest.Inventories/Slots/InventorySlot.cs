using TheChest.Inventories.Slots.Interfaces;
using TheChest.Core.Slots;

namespace TheChest.Inventories.Slots
{
    /// <summary>
    /// Generic Inventory Slot with with <see cref="Slot{T}"/> extension and <see cref="IInventorySlot{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">The item type the slot accepts</typeparam>
    public class InventorySlot<T> : Slot<T>, IInventorySlot<T>
    {
        /// <summary>
        /// Creates a basic inventory slot with an item
        /// </summary>
        /// <param name="currentItem">item that belongs to this slot</param>
        public InventorySlot(T currentItem = default!) : base(currentItem) { }

        /// <inheritdoc />
        public virtual bool Add(T item)
        {
            if (this.IsFull)
            {
                return false;
            }

            this.Content = item;
            return true;
        }

        /// <inheritdoc />
        public virtual T Get()
        {
            var content = this.Content;
            this.Content = default;
            return content;    
        }

        /// <inheritdoc />
        public virtual T Replace(T item)
        {
            var content = this.Content;
            this.Content = item;
            return content;
        }
    }
}
