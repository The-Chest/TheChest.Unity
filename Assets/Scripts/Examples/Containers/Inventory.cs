using System;
using UnityEngine;
using TheChest.Examples.Items;
using TheChest.Inventories.Containers;
using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Examples.Containers
{
    [Serializable]
    public class Inventory : Inventory<Item>
    {
        protected const string DEFAULT_CONTAINER_NAME = "CONTAINER_NAME";

        [SerializeField]
        private string containerName;
        public string ContainerName => this.containerName;

        [SerializeField]
        protected StackSlot[] slots;

        public new StackSlot[] Slots
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

        public Inventory(StackSlot[] slots, string containerName = DEFAULT_CONTAINER_NAME) : base(slots as IInventorySlot<Item>[]) 
        {
            this.containerName = containerName;
        }
    }
}
