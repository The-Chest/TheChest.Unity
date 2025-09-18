using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack.Lazy;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="ILazyStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class LazyStackInventory<T> : StackContainer<T>, ILazyStackInventory<T>
    {
        /// <inheritdoc/>
        public event LazyStackInventoryGetEventHandler<T>? OnGet;
        /// <inheritdoc/>
        public event LazyStackInventoryAddEventHandler<T>? OnAdd;
        /// <inheritdoc/>
        public event LazyStackInventoryMoveEventHandler<T>? OnMove;

        /// <summary>
        /// Creates an Stackable Inventory with lazy behavior
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventoryLazyStackSlot{T}"/></param>
        /// <exception cref="ArgumentNullException">When <paramref name="slots"/> is null</exception>
        public LazyStackInventory(IInventoryLazyStackSlot<T>[] slots) : base(slots)
        {
            this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }

        /// <summary>
        /// Slots of the inventory
        /// </summary>
        protected new readonly IInventoryLazyStackSlot<T>[] slots;
        /// <summary>
        /// Gets an slot from the inventory
        /// </summary>
        /// <param name="index">index of the slot to be returned</param>
        /// <returns></returns>
        public new IInventoryLazyStackSlot<T> this[int index] => this.slots[index];

        /// <summary>
        /// Adds an item to the first available slot
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the inventory.
        /// </remarks>
        /// <param name="item">Item to be added to the inventory</param>
        /// <returns>True if <paramref name="item"/> is possible to be added to the inventory</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual bool Add(T item)
        {
            if(item is null)
                throw new ArgumentNullException(nameof(item));

            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.CanAdd(item))
                {
                    var notAdded = slot.Add(item);
                    if (notAdded == 0)
                        this.OnAdd?.Invoke(this, (item, index, 1));

                    return notAdded == 0; 
                }
            }

            return false;
        }
        /// <summary>
        /// Adds an amount of items to the first available slot.
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when any amount of <paramref name="item"/> are added to the inventory.
        /// </remarks>
        /// <param name="item">Item to be added to the inventory</param>
        /// <param name="amount">Amount of <paramref name="item"/> to be added</param>
        /// <returns>Empty array when is succesfully added, otherwise it'll return an array with not added items</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual int Add(T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var notAddedAmount = amount;
            var events = new List<LazyStackInventoryAddItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.CanAdd(item))
                {
                    var previousAmount = notAddedAmount;
                    notAddedAmount = slot.Add(item, previousAmount);
                    var addedAmount = previousAmount - notAddedAmount;
                    events.Add(new LazyStackInventoryAddItemEventData<T>(item, index, addedAmount));
                    if (notAddedAmount == 0)
                        break;
                }
            }
            if(events.Count > 0)
                this.OnAdd?.Invoke(this, new LazyStackInventoryAddEventArgs<T>(events));

            return notAddedAmount;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the <paramref name="index"/> .
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        [Obsolete("This method will be removed in the future versions. Use AddAt(T item, int index, int amount) instead")]
        public virtual T[] AddAt(T item, int index, int amount, bool replace)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (slot.CanAdd(item, amount))
            {
                var notAdded = slot.Add(item, amount);
                this.OnAdd?.Invoke(this, (item, index, amount - notAdded));
                return Enumerable.Repeat(item, notAdded).ToArray();
            }
            else if(replace && slot.CanReplace(item, amount))
            {
                this.OnAdd?.Invoke(this, (item, index, amount));
                return slot.Replace(item, amount);
            }

            return Enumerable.Repeat(item, amount).ToArray();
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the <paramref name="index"/> .
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual int AddAt(T item, int index, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (slot.CanAdd(item, amount))
            {
                var notAdded = slot.Add(item, amount);
                this.OnAdd?.Invoke(this, (item, index, amount - notAdded));
                return notAdded;
            }

            return amount;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when all items are returned from the inventory.
        /// </remarks>
        public virtual T[] Clear()
        {
            var events = new List<LazyStackInventoryGetItemEventData<T>>();
            var items = new List<T>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (!slot.IsEmpty)
                {
                    var slotItems = slot.GetAll();
                    if (slotItems.Length > 0)
                        events.Add(new LazyStackInventoryGetItemEventData<T>(slotItems[0], index, slotItems.Length));

                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new LazyStackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when an item is returned from <paramref name="index"/>.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T Get(int index)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = this.slots[index].Get().FirstOrDefault();
            if (!EqualityComparer<T>.Default.Equals(item, default!))
                this.OnGet?.Invoke(this, (item, index, 1));

            return item;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when <paramref name="item"/> is returned from the inventory.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T Get(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var foundItem = slot.Get().FirstOrDefault();
                    if(!EqualityComparer<T>.Default.Equals(foundItem, default!))
                        this.OnGet?.Invoke(this, (foundItem, index, 1));

                    return foundItem;
                }
            }

            return default;
        }
        /// <summary>
        /// Gets an amount of items from the inventory
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when any amount of <paramref name="item"/> is returned from the inventory.
        /// </remarks>
        /// <param name="item">Item to be searched on the inventory</param>
        /// <param name="amount">Amount of <paramref name="item"/> to be returned</param>
        /// <returns>The amount of items searched (or the max it can return)</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = new List<T>();
            var events = new List<LazyStackInventoryGetItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var slotItems = slot.Get(amount);
                    if (slotItems.Length > 0)
                        events.Add(new LazyStackInventoryGetItemEventData<T>(slotItems[0], index, slotItems.Length));

                    items.AddRange(slotItems);
                    amount -= slotItems.Length;
                    if (amount == 0)
                        break;
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new LazyStackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }
        /// <summary>
        /// Gets an amount of items from an specific slot the inventory
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when any amount of item is returned from <paramref name="index"/> of the inventory.
        /// </remarks>
        /// <param name="index">Slot item index to be returned</param>
        /// <param name="amount">Amount to be returned (or the max available)</param>
        /// <returns>An array of items</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T[] Get(int index, int amount)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = this.slots[index].Get(amount);
            if (items.Length > 0)
                this.OnGet?.Invoke(this, (items[0], index, items.Length));

            return items;
        }
        /// <summary>
        /// Gets all items of the selected type from all slots
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when every <paramref name="item"/> is returned from the inventory.
        /// </remarks> 
        /// <param name="item">Item to be searched</param>
        /// <returns>A list with all items founded in the inventory</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] GetAll(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var events = new List<LazyStackInventoryGetItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var slotItems = slot.GetAll();
                    if (slotItems.Length > 0)
                        events.Add(new LazyStackInventoryGetItemEventData<T>(slotItems[0], index, slotItems.Length));
                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new LazyStackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when all items from <paramref name="index"/> is returned from the inventory.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T[] GetAll(int index)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var items = this.slots[index].GetAll();
            if (items.Length > 0)
                this.OnGet?.Invoke(this, (items[0], index, items.Length));

            return items;
        }
        /// <summary>
        /// Returns the amount of an item inside the inventory
        /// </summary>
        /// <param name="item">Item to be searched</param>
        /// <returns>The amount of the <paramref name="item"/> in the Inventory </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual int GetCount(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var count = 0;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                {
                    count += slot.Amount;
                }
            }
            return count;
        }
        /// <summary>
        /// Moves an item from one slot to another
        /// </summary>
        /// <param name="origin">Origin slot that will be moved to <paramref name="target"/></param>
        /// <param name="target">Target slot that will be moved to <paramref name="origin"/></param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> index are smaller than zero or bigger than <see cref="StackContainer{T}.Size"/></exception>
        /// <exception cref="ArgumentException">When <paramref name="origin"/> and <paramref name="target"/> are equal</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin > this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target > this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            if (origin == target)
                throw new ArgumentException("Origin and target cannot be the same");

            var originSlot = this.slots[origin];
            var targetSlot = this.slots[target];
            if (originSlot.IsEmpty && targetSlot.IsEmpty)
                return;

            var events = new List<LazyStackInventoryMoveItemEventData<T>>();
            var originItems = originSlot.GetAll();
            var originItem = originItems.FirstOrDefault();

            if (!EqualityComparer<T>.Default.Equals(originItem, default!))
            {
                var targetItems = targetSlot.Replace(originItem!, originItems.Length);
                events.Add(new LazyStackInventoryMoveItemEventData<T>(originItem!, originItems.Length, origin, target));
                var targetItem = targetItems.FirstOrDefault();

                if(!EqualityComparer<T>.Default.Equals(targetItem, default!))
                {
                    originSlot.Replace(targetItem!, targetItems.Length);
                    events.Add(new LazyStackInventoryMoveItemEventData<T>(targetItem!, targetItems.Length, target, origin));
                }
            }
            else
            {
                var targetItems = targetSlot.GetAll();
                var targetItem = targetItems.FirstOrDefault();
                if (!EqualityComparer<T>.Default.Equals(targetItem, default!))
                {
                    originSlot.Add(targetItem!, targetItems.Length);
                    events.Add(new LazyStackInventoryMoveItemEventData<T>(targetItem!, targetItems.Length, target, origin)); 
                }
            }

            this.OnMove?.Invoke(this, new LazyStackInventoryMoveEventArgs<T>(events.ToArray()));
        }
    }
}
