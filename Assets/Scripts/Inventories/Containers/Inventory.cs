using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class Inventory<T> : Container<T>, IInventory<T>
    {
        protected readonly IInventorySlot<T>[] slots;

        /// <summary>
        /// Creates an Inventory with <see cref="IInventorySlot{T}"/> implementation
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventorySlot{T}"/></param>
        public Inventory(IInventorySlot<T>[] slots) : base(slots) 
        {
            this.slots = slots;
        }

        /// <inheritdoc/>
        public virtual T[] Add(params T[] items)
        {
            if (items.Length == 0) 
                return items;

            var addedAmount = 0;
            var index = 0;
            while (index < this.Size)
            {
                if(addedAmount >= items.Length)
                    break;

                var item = items[addedAmount];
                if (item is null)
                {
                    addedAmount++;
                    continue;
                }

                var added = this.slots[index].Add(item);
                if (added)
                    addedAmount++;

                index++;
            }

            if (addedAmount < items.Length)
            {
                return items.Skip(addedAmount).ToArray();
            }

            return Array.Empty<T>();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual bool Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size ; i ++)
            {
                var added = this.slots[i].Add(item);
                if (added)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Inventory{T}.Size"/></exception>
        public virtual T AddAt(T item, int index, bool replace = true)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            T result = default;
            if (replace)
            {
                result = this.slots[index].Replace(item);
            }
            else
            {
                var added = this.slots[index].Add(item);
                if (!added)
                {
                    result = item;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public virtual T[] Clear()
        {
            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                var item = this.slots[i].Get();
                if(item != null)
                {
                    items.Add(item);
                }
            }
            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] GetAll(T item)
        {
            if(item is null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    items.Add(this.slots[i].Get()!);
                }
            }
            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Inventory{T}.Size"/></exception>
        public virtual T Get(int index)
        {
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.slots[index].Get();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T Get(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    return this.slots[i].Get();
                }
            }
            
            return default;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                if (!this.slots[i].Contains(item))
                    continue;

                var slotItem = this.slots[i].Get();
                if(slotItem is null)
                    continue;

                items.Add(slotItem);

                if (items.Count == amount)
                    break;
            }
            return items.ToArray();
        }
        
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual int GetCount(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var count = 0;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="origin"><inheritdoc/></param>
        /// <param name="target"><inheritdoc/></param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are smaller than zero or bigger than the container size</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            var item = this.slots[origin].Get();

            var oldItem = this.slots[target].Replace(item);
            this.slots[origin].Replace(oldItem);
        }
    }
}
