using System;
using UnityEngine;

namespace TheChest.Examples.Items
{
    [Serializable]
    public class Item
    {
        //Maybe just keep properties 
        [Header("Basic item data")]

        [SerializeField]
        protected string id;

        [SerializeField]
        protected string name;

        [SerializeField]
        protected string description;

        [SerializeField]
        protected Sprite image;

        [SerializeField]
        [Range(0,100)]
        protected int maxStack;

        /// <summary>
        /// Unique identifier of item 
        /// </summary>
        public string ID => id;

        /// <summary>
        /// Name of item
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Description of Item
        /// </summary>
        public string Description => description;

        /// <summary>
        /// Image of item
        /// </summary>
        public Sprite Image => image;

        /// <summary>
        /// Max stack of item on slot
        /// </summary>
        public int MaxStack => maxStack;

        public Item()
        {
            id = null;
            name = null;
            description = null;
            image = null;
            maxStack = 1;
        }

        public Item(string id, string name, string description, Sprite image, int maxStack)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.image = image;
            this.maxStack = maxStack;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
                return false;
            return obj is Item item &&
                   ID == item.ID &&
                   Name == item.Name &&
                   MaxStack == item.MaxStack;
        }
    }
}
