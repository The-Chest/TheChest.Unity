using UnityEngine;
using TheChest.Examples.Items;
using TheChest.Inventories.Containers;

namespace TheChest.Examples.Containers
{
    public class StackInventory : StackInventory<Item>
    {
        [SerializeField]
        protected new StackSlot[] slots;

        public StackInventory(StackSlot[] slots) : base(slots)
        {
        }
    }
}
