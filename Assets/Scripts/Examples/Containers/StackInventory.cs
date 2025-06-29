using UnityEngine;
using TheChest.Examples.Items;
using TheChest.Inventories.Containers;
using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Examples.Containers
{
    public class StackInventory : StackInventory<Item>
    {
        [SerializeField]
        protected StackSlot[] slots;

        public override IStackSlot<Item>[] Slots
        {
            get
            {
                return (IStackSlot<Item>[])slots;
            }
            protected set
            {
                slots = value as StackSlot[];
            }
        }

        public StackInventory(StackSlot[] slots) : base(slots as IInventoryStackSlot<Item>[])
        {
        }
    }
}
