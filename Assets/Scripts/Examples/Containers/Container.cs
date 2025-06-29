using UnityEngine;
using TheChest.Examples.Items;
using TheChest.Core.Containers;
using TheChest.Core.Slots.Interfaces;

namespace TheChest.Examples.Containers
{
    public class Container : Container<Item>
    {
        [SerializeField]
        protected StackSlot[] slots;

        public Container(ISlot<Item>[] slots) : base(slots)
        {
        }
    }
}
