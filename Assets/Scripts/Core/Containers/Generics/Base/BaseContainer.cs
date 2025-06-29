using System;
using System.Linq;
using TheChest.Containers.Generics.Interfaces;
using TheChest.Slots.Generics.Interfaces;

namespace TheChest.Containers.Generics.Base
{
    /// <summary>
    /// Generic container with <see cref="IContainer{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public abstract class BaseContainer<T> : IContainer<T>
    {
        protected const int DEFAULT_SLOT_COUNT = 20;

        public virtual ISlot<T>[] Slots {
            get ;
            protected set;
        }

        public virtual ISlot<T> this[int index] => this.Slots[index];

        public virtual int Size => Slots.Length;

        public virtual bool IsFull => this.Slots.All(x => x.IsFull);

        public virtual bool IsEmpty => this.Slots.All(x => x.IsEmpty);

        /// <summary>
        /// Creates a Container with slots
        /// </summary>
        /// <param name="slots">An array of slots</param>
        protected BaseContainer(ISlot<T>[] slots)
        {
            Slots = slots;
        }

        /// <summary>
        /// Creates a Container with a number of slots
        /// </summary>
        /// <param name="size">Sets the amount of slots (20 if not set)</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected BaseContainer(int size = DEFAULT_SLOT_COUNT)
        {
            if(size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), $"Invalid Container Size : {size}");
            }
            Slots = new ISlot<T>[size];
        }
    }
}
