using NUnit.Framework;
using System;
using TheChest.Examples.Containers;
using TheChest.Examples.Items;

namespace TheWorld.Tests.TheChest
{
    public partial class InventoryTests
    {
        public Random random;

        const int low_amount = 10;
        const int high_amount = 20;

        const int low_size = 10;
        const int high_size = 20;

        public InventoryTests()
        {
            random = new Random();

            Assert.IsTrue(low_amount < high_amount);
        }

        #region Generators
        private Item DefaultItemGenerator(string id = null,string name = null, string description = null , int maxStack = -1)
        {
            return new Item(
              id: id??Guid.NewGuid().ToString(),
              name: name??Guid.NewGuid().ToString(),
              description: description??Guid.NewGuid().ToString(),
              image: null,
              maxStack: maxStack > 0? maxStack : random.Next(1, high_amount)
           );
        }

        private Item DistinctItemGenerator(int maxStack = -1)
        {
            return new Item(
              id: Guid.NewGuid().ToString() + random.Next(0,10),
              name: Guid.NewGuid().ToString() + random.Next(0, 10),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: maxStack > 0 ? maxStack : random.Next(1, high_amount)
           );
        }

        private StackSlot DefaultSlotGenerator(bool isEmpty = true,int amount = high_amount)
        {
            if(isEmpty)
                return new StackSlot();
            else
                return new StackSlot(this.DefaultItemGenerator(), amount);
        }

        private Inventory DefaultInventoryGenerator(bool isEmpty = true, int slotAmount = 20 , int itemAmount = high_amount, Item itemTemplate = null)
        {
            if (isEmpty)
            {
                return new Inventory();
            }
            else
            {
                var slots = new StackSlot[slotAmount];

                for (int i = 0; i < slots.Length; i++)
                {
                    if(itemTemplate == null)
                    {
                        slots[i] = new StackSlot(this.DefaultItemGenerator(maxStack: itemAmount));
                    }
                    else
                    {
                        slots[i] = new StackSlot(itemTemplate,itemAmount);
                    }
                }

                var inventory = new Inventory(slots);

                return inventory;
            }
        }
        #endregion

        [Test]
        public void InventoryConstructorAmount()
        {
            var amount = random.Next(low_amount,high_amount);
            var inventory = new Inventory(amount);
            Assert.AreEqual(amount, inventory.Slots.Length);
        }

        [Test]
        public void InventoryConstructorNegativeAmount()
        {
            var amount = random.Next(-high_size, -low_size);
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Inventory(amount), $"Invalid Container Size : {amount}");
        }
    }
}
