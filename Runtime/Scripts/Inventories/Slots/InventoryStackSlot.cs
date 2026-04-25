using System;
using System.Linq;
using System.Collections.Generic;
using TheChest.Core.Slots;
using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Slots
{
    /// <summary>
    /// Slot with with <see cref="IInventoryStackSlot{T}"/> implementation with a collection of items
    /// </summary>
    /// <typeparam name="T">The item collection inside the slot accepts</typeparam>
    public class InventoryStackSlot<T> : StackSlot<T>, IInventoryStackSlot<T>
    {
        /// <inheritdoc />
        public virtual int AvailableAmount => this.maxAmount - this.amount;

        /// <summary>
        /// Creates an Inventory Slot with default items stacked
        /// </summary>
        public InventoryStackSlot(T[] items) : base(items) { }
        /// <summary>
        /// Creates an Inventory Slot with default items stacked
        /// </summary>
        public InventoryStackSlot(T[] items, int maxStackAmount) : base(items, maxStackAmount) { }

        /// <summary>
        /// Adds an array of items inside the Content with no previous validation.
        /// <para>
        /// It's recommended to use <see cref="IInventoryStackSlot{T}.Add(T[])"/> or <see cref="IInventoryStackSlot{T}.CanAdd(T[])"/> to ensure no invalid items are added
        /// </para>
        /// </summary>
        /// <param name="items">items to be added to the slot (and the reference will be removed after)</param>
        protected virtual void AddItems(ref T[] items)
        {
            var addAmount = items.Length > this.AvailableAmount ? this.AvailableAmount : items.Length;

            var itemIndex = 0;
            for (int i = 0; i < this.MaxAmount; i++)
            {
                if (this.content[i] is null)
                {
                    this.content[i] = items[itemIndex++];
                    this.amount++;
                }

                if (itemIndex == addAmount)
                    break;
            }

            items = items.Skip(addAmount).ToArray();
        }
        /// <summary>
        /// Adds an item inside the Content with no previous validation.
        /// </summary>
        /// <param name="item">item to be added to content</param>
        protected virtual void AddItem(ref T item)
        {
            for (int i = 0; i < this.MaxAmount; i++)
            {
                if (this.content[i] is null)
                {
                    this.content[i] = item;
                    this.amount++;
                    break;
                }
            }
            item = default!;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <remarks>
        /// This method checks whether the slot is full or already contains the specified item. 
        /// <para>
        /// Override this method to customize the criteria for adding <paramref name="item"/>.
        /// </para>
        /// </remarks>
        /// <param name="item"><inheritdoc/></param>
        /// <returns>true if the item can be added to the slot; otherwise, false.</returns>
        public virtual bool CanAdd(T item)
        {
            if (item is null)
                return false;

            if (this.IsFull)
                return false;

            if (!this.IsEmpty)
                return this.Contains(item);

            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <remarks>
        /// This method checks that the slot is not full, that the number of items does not
        /// exceed the available capacity, and that all items are non-null and equal to each other.  
        /// If the slot is not empty, the items must also match the type of items already contained.   
        /// The method does not modify <paramref name="items"/>.
        /// <para>
        /// Override this method to customize the criteria for adding items.
        /// </para>
        /// </remarks>
        /// <param name="items"><inheritdoc/></param>
        /// <returns>true if all items can be added to the slot; otherwise, false.</returns>
        public virtual bool CanAdd(T[] items)
        {
            if (items == null || items.Length == 0)
                return false;

            if (this.IsFull)
                return false;

            if (items.Length > this.AvailableAmount)
                return false;

            var firstItem = items[0];
            if (firstItem is null)
                return false;

            if (!this.IsEmpty && !this.Contains(firstItem))
                return false;

            for (int i = 1; i < items.Length; i++)
            {
                if (items[i] is null)
                    return false;

                if(!firstItem.Equals(items[i]))
                    return false;
            }

            return true;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The items must be the same in it and in the slot (if is not empty) or it'll throw an <see cref="ArgumentException"/>. 
        /// </remarks>
        /// <exception cref="ArgumentException">When the item array is empty or has different items inside it or has any that is not equal to the items inside the slot</exception>
        public virtual T[] Add(T[] items)
        {
            //TODO: improve this method validations
            if (items.Length == 0)
                throw new ArgumentException("Cannot add empty list of items", nameof(items));

            for (int i = 1; i < items.Length; i++)
            {
                if (!items[0]!.Equals(items[i]))
                    throw new ArgumentException($"Param \"items\" have items that are not equal ({i})", nameof(items));

                if (!this.IsEmpty && !this.Contains(items[i]))
                    throw new ArgumentException($"Param \"items\" must have every item equal to the Current item on the Slot ({i})", nameof(items));
            } 

            this.AddItems(ref items);

            return items;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">when <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool Add(T item)
        {
            if(item is null)
                throw new ArgumentNullException(nameof(item));

            if (this.CanAdd(item))
            {
                this.AddItem(ref item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets and removes amount of items from slot with no previous validation.
        /// </summary>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>An list with the size of <paramref name="amount"/> or the max possible</returns>
        protected virtual T[] GetItems(int amount)
        {
            // TODO: improve it by getting it from the last items (maybe using IEnumerable)
            // Turn it in a internal extension method
            var result = this.content
                .Where(x => !EqualityComparer<T>.Default.Equals(x, default!))
                .Take(amount)
                .ToArray();
            
            Array.Clear(this.content, 0, amount);

            this.amount -= result.Length;

            return result;
        }
        /// <summary>
        /// Gets and removes a single item from slot with no previous validation.
        /// </summary>
        /// <returns>One item or <see langword="null"/> if not found</returns>
        protected virtual T GetItem()
        {
            var item = this.content.FirstOrDefault();
            Array.Clear(this.content, 0, 1);
            if (item is null)
                return default!;

            this.amount--;
            return item!;
        }

        /// <summary>
        /// Gets and removes all items from slot
        /// </summary>
        /// <returns>All items from slot</returns>
        public virtual T[] GetAll()
        {
            return this.GetItems(this.Amount);
        }
        /// <summary>
        /// Gets an removes amount of items from slot.
        /// If is bigger than <see cref="IStackSlot{T}.Amount"/> it returns the maximum amount possible.
        /// </summary>
        /// <param name="amount">Amount of items to get from slot</param>
        /// <returns>An array with the max amount possible from slot</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (amount >= this.Amount)
                return this.GetAll();

            return this.GetItems(amount);
        }
        /// <summary>
        /// Gets a single item from inside the slot
        /// </summary>
        /// <returns>an item from slot or <see langword="null"/> if <see cref="ISlot{T}.IsEmpty"/> is true</returns>
        public virtual T Get()
        {
            if (this.IsEmpty)
                return default!;

            return this.GetItem();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <returns>false if the array is bigger than <see cref="IStackSlot{T}.MaxAmount"/> or is empty</returns>
        public virtual bool CanReplace(T[] items)
        {
            if (items.Length == 0)
                return false;
            if (items.Length > this.MaxAmount)
                return false;

            var firstItem = items[0]!;
            for (int i = 0; i < items.Length; i++)
            {
                if (!this.CanReplace(items[i]))
                    return false;

                if (!firstItem.Equals(items[i]))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns>false if the param <paramref name="item"/> is <see langword="null"/></returns>
        public virtual bool CanReplace(T item)
        {
            if (item is null)
                return false;

            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="items"/> dize is zero or bigger than <see cref="IStackSlot{T}.MaxAmount"/></exception>
        /// <exception cref="ArgumentException">When any of items in param are invalid</exception>
        /// <returns>The current items from content or <paramref name="items"/> if is not possible to replace</returns>
        public virtual T[] Replace(T[] items)
        {
            if (items.Length == 0)
                throw new ArgumentException("Cannot replace the slot for empty item array", nameof(items));

            if (items.Length > this.MaxAmount)
                throw new ArgumentOutOfRangeException(nameof(items));

            var firstItem = items[0]!;
            for (int i = 1; i < items.Length; i++)
            {
                if (!firstItem.Equals(items[i]))
                {
                    throw new ArgumentException($"Param \"items\" have items that are not equal ({i})", nameof(items));
                }
            }

            if(this.IsEmpty) 
            {
                this.Add(items);
                return Array.Empty<T>();
            }

            var result = this.GetAll();
            if (this.CanAdd(items))
            {
                this.Add(items);
                return result; 
            }

            this.AddItems(ref result);
            return items;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item">the item that will be attempt to replace</param>
        /// <returns><see langword="null"/> if the slot is empty. The items from inside the slot if is not empty and possible to replace. An array with <paramref name="item"/> if is not possible to replace</returns>
        /// <exception cref="ArgumentNullException">when <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T[] Replace(T item)
        {
            if(item is null)
                throw new ArgumentNullException(nameof(item));

            if (this.IsEmpty)
            {
                this.AddItem(ref item);
                return Array.Empty<T>();
            }

            var result = this.GetAll();
            if (this.CanAdd(item))
            {
                this.Add(item);
                return result;
            }

            this.AddItems(ref result);
            return new T[1]{ item };
        }
    }
}
