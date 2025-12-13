using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Slots;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Slots
{
    /// <summary>
    /// Class with methods for a basic Inventory Stackable Slot with lazy behavior
    /// <para>
    /// Warning: this class has some properties that will soon be moved to <see cref="LazyStackSlot{T}"/> because of LazyStackSlot not being lazyyet.
    /// </para>
    /// </summary>    
    /// <typeparam name="T">Item the Slot Accept</typeparam>
    public class InventoryLazyStackSlot<T> : LazyStackSlot<T>, IInventoryLazyStackSlot<T>
    {
        /// <inheritdoc/>
        public override bool IsFull => 
            !EqualityComparer<T>.Default.Equals(this.content, default!) && 
            this.Amount == this.MaxAmount;
        /// <inheritdoc/>
        public override bool IsEmpty => 
            this.content is null || 
            this.Amount == 0;

        /// <summary>
        /// Creates an Inventory Stackable Slot with lazy behavior
        /// </summary>
        /// <param name="content">default item inside the slot</param>
        /// <param name="amount">amount of the <paramref name="amount"/></param>
        /// <param name="maxStackAmount">the max accepted amount of this slot</param>
        public InventoryLazyStackSlot(T content = default!, int amount = 1, int maxStackAmount = 1) : base(content, amount, maxStackAmount)
        {
            this.content = content;
        }

        /// <summary>
        /// Clears the slot by removing the content and setting the amount to 0
        /// </summary>
        protected void Clear()
        {
            this.content = default!;
            this.Amount = 0;
        }
        /// <summary>
        /// Gets the content of the slot as an array with the amount of items inside the slot
        /// </summary>
        /// <param name="amount">Amount of items to be returned</param>
        /// <returns>Returns an array of items from the slot</returns>
        protected T[] GetContent(int amount)
        {
            if (this.IsEmpty)
                return Array.Empty<T>();
            if (this.Amount <= amount)
            {
                var items = Enumerable.Repeat(this.content, this.Amount).ToArray();
                this.Clear();
                return items;
            }
            this.SetContent(this.content, this.Amount - amount);

            return Enumerable.Repeat(this.content!, amount).ToArray();
        }
        /// <summary>
        /// Sets the values of content and <see cref="StackSlot{T}.Amount"/>
        /// </summary>
        /// <param name="item">The value to be set to content</param>
        /// <param name="amount">The value to be set to <see cref="StackSlot{T}.Amount"/></param>
        protected void SetContent(T item, int amount)
        {
            this.content = item;
            this.Amount = amount;
        }

        /// <summary>
        /// Adds an amount of items to the slot.
        /// <para>
        /// This method doesn't validate the params and should be used only after <see cref="InventoryLazyStackSlot{T}.CanAdd(T, int)"/>
        /// </para>
        /// </summary>
        /// <param name="item">The item to be added </param>
        /// <param name="amount">The amount of items added</param>
        /// <returns>Return 0 if all items are fully added to slot, else will return the amount left</returns>
        private int AddItems(T item, int amount = 1)
        {
            var leftAmount = 0;
            if (this.Amount + amount > this.MaxAmount)
            {
                leftAmount = this.Amount + amount - this.MaxAmount;
                this.SetContent(item, this.MaxAmount);
            }
            else
            {
                this.SetContent(item, this.Amount + amount);
            }

            return leftAmount;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is smaller than zero</exception>
        public virtual int Add(T item, int amount = 1)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (this.IsFull)
                return amount;
            if (!this.IsEmpty && !this.content!.Equals(item))
                return amount;

            return this.AddItems(item, amount);
        }

        /// <inheritdoc/>.
        /// <remarks>
        /// If the slot is not empty and the item is equal to the current item it will return true
        /// </remarks>
        /// <returns>true if <paramref name="item"/> is not null and <paramref name="amount"/> is bigger than zero and the slot is not full</returns>
        public virtual bool CanAdd(T item, int amount = 1)
        {
            if(item is null)
                return false;

            if (this.IsFull)
                return false;

            if (amount <= 0)
                return false;

            if (!this.IsEmpty)
                return this.content!.Equals(item);

            return true;
        }

        /// <inheritdoc/>
        /// <returns>true if <paramref name="item"/> is not null and <paramref name="amount"/> is bigger than zero and smaller than <see cref="LazyStackSlot{T}.MaxAmount"/></returns>
        public virtual bool CanReplace(T item, int amount = 1)
        {
            if (item is null)
                return false;

            if (amount <= 0 || amount > this.MaxAmount)
                return false;

            return true;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// If the slot is Empty, it'll try to add the max possible amount of items and returning the amount left
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is smaller than zero or bigger than <see cref="LazyStackSlot{T}.MaxAmount"/></exception>
        public virtual T[] Replace(T item, int amount = 1)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0 || amount > this.MaxAmount)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (this.IsEmpty)
            {
                var left = this.AddItems(item, amount);
                return Enumerable.Repeat(item, left).ToArray();
            }

            var slotItems = this.GetContent(this.Amount);
            this.SetContent(item,amount);
            return slotItems;
        }

        /// <summary>
        /// Gets an amount of items from the slot
        /// </summary>
        /// <param name="amount">The choosen amount to be returned</param>
        /// <returns>A list with an array with the max of items or max that the slot could retrieve</returns>
        /// <exception cref="ArgumentOutOfRangeException">When amount is smaller than zero</exception>
        public virtual T[] Get(int amount = 1)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return this.GetContent(amount);
        }

        /// <inheritdoc/>
        public virtual T[] GetAll()
        {
            return this.GetContent(this.Amount);
        }
    }
}
