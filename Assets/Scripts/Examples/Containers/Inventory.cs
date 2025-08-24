using System;
using UnityEngine;
using TheChest.Examples.Items;
using TheChest.Inventories.Containers;

namespace TheChest.Examples.Containers
{
    [Serializable]
    public class Inventory : StackInventory<Item>
    {
        protected const string DEFAULT_CONTAINER_NAME = "CONTAINER_NAME";

        [SerializeField]
        private string containerName;
        public string ContainerName => this.containerName;

        [SerializeField]
        protected new StackSlot[] slots;

        public StackSlot[] Slots
        {
            get
            {
                return this.slots;
            }
            protected set
            {
                slots = value;
            }
        }

        public Inventory(StackSlot[] slots, string containerName = DEFAULT_CONTAINER_NAME) : base(slots) 
        {
            this.slots = slots;
            this.containerName = containerName;
        }
    }
}
