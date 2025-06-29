using UnityEngine;
using TheChest.Examples.Items;
using TheChest.Inventories.Slots;
using System.Linq;

namespace TheChest.Examples.Containers
{
    /// <summary>
    /// Slot with stackable items and serializable Fields
    /// </summary>
    [System.Serializable]
    public class StackSlot : InventoryStackSlot<Item>
    {
        /// <summary>
        /// Current items inside the slot
        /// </summary>
        [SerializeField]
        private Item[] items;

        public override Item[] Content => this.items;

        [SerializeField]
        private int stackAmount;

        public override int StackAmount  => this.stackAmount;

        public override int MaxStackAmount => this.Content.FirstOrDefault()?.MaxStack ?? 1;

        public StackSlot(Item[] items) : base(items)
        {
        }
    }
}
