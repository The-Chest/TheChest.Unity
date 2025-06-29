using System;
using TheChest.Slots.Generics.Interfaces;

namespace TheChest.Slots.Generics.Base
{
    /// <summary>
    /// Generic Slot with with <see cref="IInventoryStackSlot{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">The item the slot accepts</typeparam>
    public abstract class BaseInventoryStackSlot<T> : BaseStackSlot<T>, IInventoryStackSlot<T>
    {
        public virtual T GetOne()
        {
            if (this.IsEmpty)
                return default;

            T item = this.CurrentItem;

            this.StackAmount--;

            if (StackAmount == 0)
                this.CurrentItem = default;

            return item;
        }

        public virtual bool Add(T item)
        {
            var eq = this.CurrentItem?.Equals(item) ?? false;

            if (this.IsEmpty || (eq && !this.IsFull))
            {
                this.CurrentItem = item;
                this.StackAmount++;
                return true;
            }

            return false;
        }

        public virtual T[] GetAmount(int amount)
        {
            if (amount < 1) return new T[0];
            if (amount > StackAmount) amount = this.StackAmount;

            T[] items = new T[amount];

            for (int i = 0; i < amount; i++)
            {
                this.StackAmount--;
                items[i] = this.CurrentItem;
            }

            if (this.StackAmount == 0) this.CurrentItem = default;

            return items;
        }

        public virtual T[] GetAll()
        {
            return this.GetAmount(this.StackAmount);
        }

        public virtual int Add(T item, int amount)
        {
            if (amount < 1)
                return 0;

            var eq = this.CurrentItem?.Equals(item) ?? false;

            if ((!this.IsEmpty && !eq) || this.IsFull)
                return amount;

            int res = 0;

            this.CurrentItem = item;

            if (amount + this.StackAmount > MaxStackAmount)
            {
                res = this.StackAmount + amount - MaxStackAmount;
                this.StackAmount = MaxStackAmount;
            }
            else
            {
                this.StackAmount += amount;
            }

            return Math.Abs(res);
        }

        public virtual int Add(T[] items)
        {
            if (items == null || items.Length == 0)
                return 0;

            var eq = this.CurrentItem?.Equals(items[0]) ?? false;

            if ((!this.IsEmpty && !eq) || this.IsFull)
                return items.Length;

            int res = 0;

            this.CurrentItem = items[0];

            if (items.Length + this.StackAmount > MaxStackAmount)
            {
                res = this.StackAmount + items.Length - MaxStackAmount;
                this.StackAmount = MaxStackAmount;
            }
            else
            {
                this.StackAmount += items.Length;
            }

            return Math.Abs(res);
        }

        public virtual T[] Replace(T item, int amount)
        {
            T[] items = new T[0];

            if (amount < 1) return items;

            var eq = this.CurrentItem?.Equals(item) ?? false;

            if (eq)
            {
                int resultAmount = this.Add(item, amount);

                items = new T[resultAmount];

                for (int i = 0; i < resultAmount; i++)
                {
                    items[i] = this.CurrentItem;
                }
            }
            else
            {
                items = this.GetAll();

                this.CurrentItem = item;
                this.StackAmount = amount;
            }

            return items;
        }

        public virtual T[] Replace(T[] items)
        {
            if (items == null || items.Length == 0)
                return this.GetAll();

            T[] retItems;

            var eq = this.CurrentItem?.Equals(items[0]) ?? false;

            if (eq)
            {
                int resultAmount = this.Add(items);

                retItems = new T[resultAmount];

                for (int i = 0; i < resultAmount; i++)
                {
                    retItems[i] = this.CurrentItem;
                }
            }
            else
            {
                retItems = this.GetAll();

                this.CurrentItem = items[0];
                this.StackAmount = items.Length;
            }

            return retItems;
        }

        public T Replace(T item)
        {
            var result = this.Replace(item, 1);

            if(result.Length > 0)
            {
                return result[0];
            }

            return default;
        }
    }
}
