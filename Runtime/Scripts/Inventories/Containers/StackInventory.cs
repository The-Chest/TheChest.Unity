using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots.Extensions;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class StackInventory<T> : StackContainer<T>, IStackInventory<T>
    {
        /// <summary>
        /// Array of <see cref="IInventoryStackSlot{T}"/> slots in the inventory
        /// </summary>
        protected new readonly IInventoryStackSlot<T>[] slots;

        /// <inheritdoc/>
        public event StackInventoryAddEventHandler<T> OnAdd;
        /// <inheritdoc/>
        public event StackInventoryGetEventHandler<T> OnGet;
        /// <inheritdoc/>
        public event StackInventoryMoveEventHandler<T> OnMove;
        /// <inheritdoc/>
        public event StackInventoryReplaceEventHandler<T> OnReplace;

        /// <summary>
        /// Creates an Inventory with <see cref="IInventoryStackSlot{T}"/> slots
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventoryStackSlot{T}"/></param>
        /// <exception cref="ArgumentNullException"><inheritdoc/></exception>
        public StackInventory(IInventoryStackSlot<T>[] slots) : base(slots) 
        {
            this.slots = slots;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool CanAdd(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].CanAdd(item))
                    return true;
            }

            return false;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/> or has one item <see langword="null"/></exception>
        public virtual bool CanAdd(params T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (items.Length == 0)
                return false;
            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), "One of the items is null"); 

            var canAddAmount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                //TODO: improve this by remove Linq usage
                var toAddItems = items
                    .Skip(canAddAmount)
                    .Take(slot.AvailableAmount)
                    .ToArray();

                if (slot.CanAdd(toAddItems)){
                    canAddAmount += toAddItems.Length;
                    if (canAddAmount >= items.Length)
                        break;
                }
            }
            
            return canAddAmount == items.Length;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>"
        public virtual bool CanAddAt(T item, int index)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.slots[index].CanAdd(item);
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/> or has one item <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>"
        public virtual bool CanAddAt(T[] items, int index)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), "One of the items is null");

            return this.slots[index].CanAdd(items);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the inventory. 
        /// </remarks>
        /// <returns>true if is possible to add the items</returns>
        /// <exception cref="ArgumentNullException">When param <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var fallbackIndex = -1;
            for (var index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (!slot.CanAdd(item))
                    continue;
                
                if (slot.Contains(item))
                {
                    var added = slot.Add(item);
                    this.OnAdd?.Invoke(this, (new[] { item }, index));
                    return added; 
                }

                if(fallbackIndex == -1)
                    fallbackIndex = index;
            }

            if(fallbackIndex != -1)
            {
                var added = this.slots[fallbackIndex].Add(item);
                this.OnAdd?.Invoke(this, (new[] { item }, fallbackIndex));
                return added;
            }

            return false;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// The method fires <see cref="OnAdd"/> event when every possible item is added to the inventory.
        /// </para>
        /// <para>
        /// Warning: this method does not accept different items in the same array. 
        /// This feature will be added in <see href="https://github.com/The-Chest/TheChest.Inventories/issues/42"/>
        /// </para>
        /// </remarks>
        /// <returns>Items from params that were not added to the inventory</returns>
        public virtual T[] Add(params T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (items.Length == 0)
                return items;
            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), "One of the items is null");

            var events = new List<StackInventoryAddItemEventData<T>>(items.Length);
            var indexes = this.slots.GetAddOrderIndexes(items);

            foreach (var index in indexes)
            {
                var slot = this.slots[index];

                var itemsToAdd = items.Take(slot.AvailableAmount).ToArray();
                var notAddedItems = slot.Add(itemsToAdd);
                var addedItemsCount = itemsToAdd.Length - notAddedItems.Length;

                if (addedItemsCount <= 0)
                    continue;

                events.Add(
                    new StackInventoryAddItemEventData<T>(
                        items.Take(addedItemsCount).ToArray(),
                        index
                    )
                );

                items = items.Skip(addedItemsCount).ToArray();
                if (items.Length == 0)
                    break;
            }

            if (events.Count > 0)
                this.OnAdd?.Invoke(this, new StackInventoryAddEventArgs<T>(events.ToArray()));

            return items;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        public virtual bool AddAt(T item, int index)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (!slot.CanAdd(item))
                return false;

            var added = slot.Add(item);
            if (added)
                this.OnAdd?.Invoke(this, (new[] { item }, index));
            
            return added;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        public virtual T[] AddAt(T[] items, int index)
        {
            if (items.Length == 0)
                return items;
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), "One of the items is null");

            var slot = this.slots[index];
            if (!slot.CanAdd(items))
                return items;

            var notAddedItems = slot.Add(items);
            if (notAddedItems.Length != items.Length)
                this.OnAdd?.Invoke(this, (items.Skip(notAddedItems.Length).ToArray(), index));

            return notAddedItems;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when every item is retrieved from the inventory. 
        /// </remarks>
        public virtual T[] Clear()
        {
            if(this.IsEmpty)
                return Array.Empty<T>();

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();

            for (int i = 0; i < this.Size; i++)
            {
                var slotItems = this.slots[i].GetAll();
                if(slotItems.Length > 0)
                    events.Add(new StackInventoryGetItemEventData<T>(slotItems, i));

                items.AddRange(slotItems);
            }
            if(events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when one item from <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        public virtual T Get(int index)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            var item = this.slots[index].Get();
            if(!EqualityComparer<T>.Default.Equals(item, default!))
                this.OnGet?.Invoke(this, (new[]{ item }, index));
            return item;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when the first item from the inventory that is equal to <paramref name="item"/> is retrieved.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T Get(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var result = slot.Get();
                    if(!EqualityComparer<T>.Default.Equals(item, default!))
                        this.OnGet?.Invoke(this, (new[]{ result }, index));
                    return result;
                }
            }
            return default!;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items in an <paramref name="amount"/> from the inventory that contains <paramref name="item"/> are retrieved.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T[] Get(T item, int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();
            var remainingAmount = amount;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                {
                    var slotItems = slot.Get(remainingAmount);
                    events.Add(new StackInventoryGetItemEventData<T>(slotItems, i));
                    items.AddRange(slotItems);
                    remainingAmount -= slotItems.Length;
                    if (remainingAmount <= 0)
                        break;
                }
            }
            if(events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events.ToArray()));    
            return items.ToArray();
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items in an <paramref name="amount"/> from the <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Inventory Size or smaller than zero</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(int index, int amount)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = this.slots[index].Get(amount);
            if(items.Length > 0)
                this.OnGet?.Invoke(this, (items, index));
            
            return items;
        }

        /// <inheritdocs/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items from <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <param name="index">The zero-based index of the collection slot from which to retrieve items.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than Inventory's Size.</exception>
        public virtual T[] GetAll(int index)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            var items = this.slots[index].GetAll();
            if (items.Length > 0)
                this.OnGet?.Invoke(this, (items, index));
            
            return items;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items from the inventory that contains <paramref name="item"/> are retrieved.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T[] GetAll(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var slotItems = slot.GetAll();
                    events.Add(new StackInventoryGetItemEventData<T>(slotItems, index));
                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual int GetCount(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var amount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                    amount += this.slots[i].Amount;
            }
            return amount;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual bool CanMove(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            
            if (origin == target)
                return false;

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                return false;
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                return false;

            return true;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            if (origin == target)
                return;

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            //TODO: improve these checks
            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                return;
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                return;

            var originItems = slotOrigin.GetAll();
            var targetItems = slotTarget.GetAll();

            var events = new List<StackInventoryMoveItemEventData<T>>();

            if (originItems.Length > 0 && slotTarget.CanAdd(originItems))
            {
                slotTarget.Add(originItems);
                events.Add(
                    new StackInventoryMoveItemEventData<T>(
                        originItems, 
                        origin, 
                        target
                    )
                );
            }

            if (targetItems.Length > 0 && slotOrigin.CanAdd(targetItems))
            {
                slotOrigin.Add(targetItems);
                events.Add(
                    new StackInventoryMoveItemEventData<T>(
                        targetItems, 
                        target,
                        origin
                    )
                );
            }

            if(events.Count > 0)
                this.OnMove?.Invoke(this, new StackInventoryMoveEventArgs<T>(events.ToArray()));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">When <paramref name="items"/> length is zero</exception>"
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Slot size or smaller than zero</exception>
        /// <exception cref="InvalidOperationException">When the amount of <paramref name="items"/> to replace exceeds the stack size of the slot on <paramref name="index"/>.</exception>
        public virtual bool CanReplace(T[] items, int index)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
 
            if (items.Length == 0)
                return false;
            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), "One of the items is null");

            return this.slots[index].CanReplace(items);
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">When <paramref name="items"/> length is zero</exception>"
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Slot size or smaller than zero</exception>
        /// <exception cref="InvalidOperationException">When the amount of <paramref name="items"/> to replace exceeds the stack size of the slot on <paramref name="index"/>.</exception>
        public virtual T[] Replace(T[] items, int index)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (items.Length == 0)
                throw new ArgumentException("Cannot replace using an empty item array", nameof(items));
            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), "One of the items is null");
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            var slot = this.slots[index];
            if (items.Length > slot.MaxAmount)
                throw new InvalidOperationException("The amount of items to replace exceeds the stack size of the slot.");

            var oldItems = slot.Replace(items);
            this.OnReplace?.Invoke(this, (index, oldItems, items));

            return oldItems;
        }
    }
}
