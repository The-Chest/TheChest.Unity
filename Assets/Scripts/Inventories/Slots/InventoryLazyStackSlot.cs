using System;
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
        protected new T content;
        public override T[] Content =>
            this.content is null ?
            Array.Empty<T>() :
            Enumerable.Repeat(this.content, this.StackAmount).ToArray();

        public override bool IsFull => this.content != null && this.StackAmount == this.MaxStackAmount;

        public override bool IsEmpty => this.content is null || this.StackAmount == 0;

        /// <summary>
        /// Creates an Inventory Stackable Slot with lazy behavior
        /// </summary>
        /// <param name="content">default item inside the slot</param>
        /// <param name="amount">amount of the <paramref name="amount"/></param>
        /// <param name="maxStackAmount">the max accepted amount of this slot</param>
        public InventoryLazyStackSlot(T content, int amount, int maxStackAmount) : base(content, amount, maxStackAmount)
        {
            this.content = content;
            this.StackAmount = amount;
            this.MaxStackAmount = maxStackAmount;
        }

        /// <summary>
        /// Clears the slot by removing the content and setting the amount to 0
        /// </summary>
        protected void Clear()
        {
            this.content = default;
            this.StackAmount = 0;
        }

        /// <summary>
        /// Sets the values of <see cref="InventoryLazyStackSlot{T}.content"/> and <see cref="InventoryLazyStackSlot{T}.StackAmount"/>
        /// </summary>
        /// <param name="item">The value to be set to <see cref="InventoryLazyStackSlot{T}.content"/></param>
        /// <param name="amount">The value to be set to <see cref="InventoryLazyStackSlot{T}.StackAmount"/></param>
        protected void SetContent(T item, int amount)
        {
            this.content = item;
            this.StackAmount = amount;
        }

        /// <summary>
        /// Adds an amount of items to the slot.
        /// <para>
        /// This method doesn't validate the params and should be used only after <see cref="InventoryLazyStackSlot{T}.CanAdd(T, int)"/>
        /// </para>
        /// <param name="item">The item to be added </param>
        /// <param name="amount">The amount of items added</param>
        /// <returns>Return 0 if all items are fully added to slot, else will return the amount left</returns>
        private int AddItems(T item, int amount = 1)
        {
            var leftAmount = 0;
            if (this.StackAmount + amount > this.MaxStackAmount)
            {
                leftAmount = this.StackAmount + amount - this.MaxStackAmount;
                this.SetContent(item, this.MaxStackAmount);
            }
            else
            {
                this.SetContent(item, this.StackAmount + amount);
            }

            return leftAmount;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <param name="amount"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
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

        /// <summary>
        /// <inheritdoc/>.
        /// <para>
        /// If the slot is not empty and the item is equal to the current item it will return true
        /// </para>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <param name="amount"><inheritdoc/></param>
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <param name="amount"><inheritdoc/></param>
        /// <returns>true if <paramref name="item"/> is not null and <paramref name="amount"/> is bigger than zero and smaller than <see cref="InventoryLazyStackSlot{T}.MaxStackAmount"/></returns>
        public virtual bool CanReplace(T item, int amount = 1)
        {
            if (item is null)
                return false;

            if (amount <= 0 || amount > this.MaxStackAmount)
                return false;

            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// <para>
        /// If the slot is Empty, it'll try to add the max possible amount of items and returning the amount left
        /// </para>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <param name="amount"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is smaller than zero or bigger than <see cref="InventoryLazyStackSlot{T}.MaxStackAmount"/></exception>
        public virtual T[] Replace(T item, int amount = 1)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0 || amount > this.MaxStackAmount)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (this.IsEmpty)
            {
                var left = this.AddItems(item, amount);
                return Enumerable.Repeat(item!, left).ToArray();
            }

            var slotItems = this.Content;
            this.SetContent(item,amount);
            return slotItems;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns>true if the slot is not empty and <paramref name="item"/> is equal to <see cref="InventoryLazyStackSlot{T}.Content"/></returns>
        public virtual bool Contains(T item)
        {
            if (this.IsEmpty)
                return false;

            return this.content?.Equals(item) ?? false;
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

            if (this.IsEmpty)
                return Array.Empty<T>();

            if(this.StackAmount < amount)
            {
                var items = this.Content;
                this.Clear();
                return items;
            }
            this.SetContent(this.content, this.StackAmount - amount);

            return Enumerable.Repeat(this.content!, amount).ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public virtual T[] GetAll()
        {
            if (this.IsEmpty)
                return Array.Empty<T>();

            var items = this.Content;
            this.Clear();
            return items;
        }
    }
}
