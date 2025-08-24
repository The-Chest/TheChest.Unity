using System;
using System.Linq;
using UnityEngine;
using TheChest.Examples.Items;
using TheChest.Inventories.Slots;
using System.Collections.Generic;

namespace TheChest.Examples.Containers
{
    /// <summary>
    /// Slot with stackable items and serializable Fields
    /// </summary>
    [Serializable]
    public class StackSlot : InventoryStackSlot<Item>
    {
        /// <summary>
        /// Current items inside the slot
        /// </summary>
        [SerializeField]
        protected new List<Item> content;

        [Obsolete("This will be removed. Use event behavior to track down inventory changes")]
        public virtual Item[] Content => this.content.ToArray();

        [SerializeField]
        protected new int stackAmount;
        public override int StackAmount
        {
            get
            {
                return this.stackAmount;
            }
            protected set
            {
                this.stackAmount = value;
            }
        }

        [SerializeField]
        protected new int maxStackAmount;
        public override int MaxStackAmount
        {
            get
            {
                if(this.IsEmpty)
                    return this.maxStackAmount;

                return this.content[0].MaxStack;
            }
            protected set
            {
                this.maxStackAmount = value;
            }
        }

        public StackSlot(Item[] items, int maxStackAmount) : base(items, maxStackAmount)
        {
            this.content = items.ToList();
        }

        /// <inheritdoc/>
        protected override void AddItems(ref Item[] items)
        {
            var availableAmount = this.MaxStackAmount - this.StackAmount;

            var addAmount = 
                items.Length > availableAmount ?
                availableAmount :
                items.Length;

            var itemIndex = 0;
            for (int i = 0; i < this.MaxStackAmount; i++)
            {
                if (itemIndex == addAmount)
                    break;

                this.content.Add(items[itemIndex]);
                itemIndex++;
            }

            this.stackAmount += addAmount;
            items = items.Skip(addAmount).ToArray();
        }

        /// <inheritdoc/>
        protected override void AddItem(ref Item item)
        {
            this.content.Add(item);
            this.stackAmount++;
            item = default!;
        }
    }
}
