using System;
using TheChest.Slots.Generics.Interfaces;

namespace TheChest.Slots.Generics.Base
{
    /// <summary>
    /// Generic Slot with with <see cref="IInventorySlot{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">The item the slot accepts</typeparam>
    public abstract class BaseInventorySlot<T> : BaseSlot<T>, IInventorySlot<T>
    {
        public virtual bool Add(T item)
        {
            var eq = this.CurrentItem?.Equals(item) ?? false;
            if (this.IsEmpty || (eq && !this.IsFull))
            {
                this.CurrentItem = item;
                return true;
            }

            return false;
        }

        public virtual T Replace(T item)
        {
            throw new NotImplementedException();
        }

        public virtual T GetOne()
        {
            if (IsEmpty)
                return default;

            T item = this.CurrentItem;
            this.CurrentItem = default;

            return item;
        }
    }
}
