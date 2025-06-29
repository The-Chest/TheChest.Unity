using System;
using TheChest.Slots.Generics.Interfaces;

namespace TheChest.Slots.Generics.Base
{
    /// <summary>
    /// Generic Slot with with <see cref="IStackSlot{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">The item the slot accepts</typeparam>
    public abstract class BaseStackSlot<T> : BaseSlot<T>, IStackSlot<T>
    {
        private const string PROPERTY_SMALLER_THAN_ZERO = "The property cannot be smaller than zero";
        private const string AMOUNT_BIGGER_THAN_MAXAMOUNT = "The amount cannot be bigger than maxAmount";

        public virtual int StackAmount { get; protected set; }

        public virtual int MaxStackAmount { get; protected set; }

        public override bool IsFull => StackAmount == MaxStackAmount && !this.IsEmpty;

        public override bool IsEmpty => CurrentItem == null || StackAmount == 0;

        /// <summary>
        /// Creates a basic Stack Slot with an amount and max amount
        /// </summary>
        /// <param name="currentItem">The current item to be added</param>
        /// <param name="amount">The amount of <paramref name="currentItem"/> to be added</param>
        /// <param name="maxStackAmount">The maximum permited amount of <paramref name="currentItem"/> to be added</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected BaseStackSlot(T currentItem = default, int amount = 1, int maxStackAmount = 1) : base(currentItem)
        {
            if (currentItem == null)
            {
                amount = 0;
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), PROPERTY_SMALLER_THAN_ZERO);
            }

            if (maxStackAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxStackAmount), PROPERTY_SMALLER_THAN_ZERO);
            }

            if(amount > maxStackAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(maxStackAmount), AMOUNT_BIGGER_THAN_MAXAMOUNT);
            }

            this.StackAmount = amount;
            this.MaxStackAmount = maxStackAmount;
        }

        /// <summary>
        /// Creates a basic Stack Slot based on a item array
        /// </summary>
        /// <param name="items">The items used to be added to</param>
        /// <param name="maxStack">The maximum permited amount of <paramref name="items"/> to be added</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected BaseStackSlot(T[] items, int maxStack)
        {
            if (maxStack < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxStack), PROPERTY_SMALLER_THAN_ZERO);
            }

            if (items != null && items.Length == 0)
            {
                this.CurrentItem = items[0];

                if (items.Length > maxStack)
                {
                    throw new ArgumentOutOfRangeException(nameof(items), AMOUNT_BIGGER_THAN_MAXAMOUNT);
                }
            }

            this.StackAmount = items?.Length ?? 0;
            this.MaxStackAmount = maxStack;
        }
    }
}
