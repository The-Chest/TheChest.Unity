using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Examples.Items;
using TheChest.Inventories.Slots;
using UnityEngine;

namespace TheChest.Examples.Containers
{
    /// <summary>
    /// Slot with stackable items and serializable Fields
    /// </summary>
    [Serializable]
    public class StackSlot : InventoryStackSlot<Item>, ISerializationCallbackReceiver
    {
        private const int DEFAULT_MAX_STACK = 10;
        /// <summary>
        /// Current items inside the slot
        /// </summary>
        [SerializeField]
        protected Item[] items;
        /// <summary>
        /// Current items inside the slot
        /// </summary>
        [Obsolete("This will be removed. Use event behavior to track down inventory changes")]
        public virtual Item[] Content => this.content;

        /// <summary>
        /// Current stack amount of the slot.
        /// </summary>
        [SerializeField]
        protected int itemAmount;
        /// <summary>
        /// Current stack amount of the slot.
        /// </summary>
        public override int StackAmount => this.Content?.Count(x => !EqualityComparer<Item>.Default.Equals(x, default!)) ?? 0;
        /// <summary>
        /// Max stack amount of the slot.
        /// </summary>
        [SerializeField]
        protected int maxItemAmount;
        /// <summary>
        /// Max stack amount of the slot. 
        /// </summary>
        /// <remarks>If empty, returns the default max stack amount, else returns the max stack of the first item in the slot</remarks>
        /// <remarks>This property for now has some extra logic that will be removed soon</remarks>
        public override int MaxStackAmount
        {
            get
            {
                if(this.IsEmpty)
                    return this.maxStackAmount;

                return this.content.FirstOrDefault(x => !(x is null))?.MaxStack ?? DEFAULT_MAX_STACK;
            }
            protected set
            {
                this.maxStackAmount = value;
            }
        }

        public StackSlot(Item[] items, int maxStackAmount) : base(items, maxStackAmount) { }

        protected override void AddItem(ref Item item)
        {
            if(this.content.Length + 1 <= this.maxStackAmount)
                Array.Resize(ref this.content, this.content.Length + 1);
            base.AddItem(ref item);
        }

        protected override void AddItems(ref Item[] items)
        {
            var newAmount = this.content.Length + items.Length;

            if(newAmount > this.maxStackAmount)
                Array.Resize(ref this.content, this.maxStackAmount);
            else if (newAmount <= this.maxStackAmount)
                Array.Resize(ref this.content, newAmount);

            base.AddItems(ref items);
        }

        public virtual void OnBeforeSerialize()
        {
            this.items = this.content;
            this.itemAmount = this.StackAmount;
            this.maxItemAmount = this.MaxStackAmount;
        }

        public virtual void OnAfterDeserialize()
        {
            this.content = this.items;
            //this.stackAmount = this.itemAmount;
            this.maxStackAmount = this.maxItemAmount;
        }
    }
}
