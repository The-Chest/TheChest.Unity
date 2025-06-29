using System.Linq;
using TheChest.Containers.Generics.Interfaces;
using TheChest.Slots.Generics.Interfaces;
using TheChest.Slots.Generics.Base;

namespace TheChest.Containers.Generics.Base
{
    /// <summary>
    /// Generic Inventory with <see cref="IStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public abstract class BaseStackInventory<T> : BaseInventory<T>, IStackInventory<T>
    {
        private IInventoryStackSlot<T>[] slots => this.Slots as IInventoryStackSlot<T>[];

        protected BaseStackInventory(int count) : base(count)
        {
        }

        protected BaseStackInventory(IInventoryStackSlot<T>[] slots) : base(slots as BaseInventorySlot<T>[])
        {
        }

        public override bool MoveItem(int origin, int target)
        {
            var oldItems = this.GetAll(origin);
            var res = this.AddItemAt(oldItems, target);
            this.AddItemAt(res, origin);
            return true;
        }

        public override T[] AddItem(T item, int amount)
        {
            if (amount < 1) 
                return new T[0];

            var itemArr = Enumerable.Repeat(item, amount).ToArray();

            for (int i = 0; i < Slots.Length; i++)
            {
                if (this.Slots[i].IsEmpty || (!this.Slots[i].IsFull && this.Slots[i].CurrentItem.Equals(item)))
                {
                    var result = this.slots[i].Add(item, amount);
                    amount = result;
                    itemArr = Enumerable.Repeat(item, result).ToArray();
                }

                if (itemArr.Length == 0)
                    break;
            }

            return itemArr;
        }

        public override T[] AddItem(T[] items)
        {
            if (items == null)
                return new T[0];

            var itemArr = items.Clone() as T[];
            var item = items[0];

            for (int i = 0; i < Slots.Length; i++)
            {
                if (this.Slots[i].IsEmpty || (!this.Slots[i].IsFull && this.Slots[i].CurrentItem.Equals(item)))
                {
                    var result = this.slots[i].Add(itemArr);
                    itemArr = Enumerable.Repeat(item, result).ToArray();
                }

                if (itemArr.Length == 0)
                    break;
            }

            return itemArr;
        }

        public virtual T[] AddItemAt(T item, int index, int amount, bool replace = true)
        {
            if (amount < 1)
                return new T[0];

            if (index < 0 || index >= Slots.Length)
                return Enumerable.Repeat(item, amount).ToArray();

            if (this.Slots[index].IsEmpty || (!this.Slots[index].IsFull && this.Slots[index].CurrentItem.Equals(item)))
            {
                var result = this.slots[index].Add(item, amount);
                return Enumerable.Repeat(item, result).ToArray();
            }
            else if (replace)
            {
                return this.slots[index].Replace(item, amount);
            }

            return Enumerable.Repeat(item, amount).ToArray();
        }

        public virtual T[] AddItemAt(T[] items, int index, bool replace = true)
        {
            if (index < 0 || index >= Slots.Length)
                return items;

            if (items == null)
                return new T[0];

            var item = items.FirstOrDefault();
            var eq = this.Slots[index].CurrentItem?.Equals(item) ?? false;

            if (this.Slots[index].IsEmpty || (!this.Slots[index].IsFull && eq))
            {
                var res = this.slots[index].Add(items);
                return Enumerable.Repeat(item, res).ToArray();
            }
            else if (replace)
            {
                return this.slots[index].Replace(items);
            }
            return items;
        }

        public virtual T[] GetAll(int index)
        {
            if (index < 0 || index >= Slots.Length) 
                return new T[0];

            return this.slots[index].GetAll();
        }

        public virtual T[] GetItemAmount(int index, int amount)
        {
            if (index < 0 || index >= Slots.Length) 
                return new T[0];

            return this.slots[index].GetAmount(amount);
        }
    }
}
