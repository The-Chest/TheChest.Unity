using NUnit.Framework;
using System;
using System.Linq;
using TheChest.Examples.Containers;
using TheChest.Examples.Items;

namespace TheWorld.Tests.TheChest
{
    public partial class InventoryTests
    {
        #region GetItem(Index)
        [Test]
        public void GetItem_from_index__Should_return_Item()
        {
            var amount = random.Next(low_amount,high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var inventory = new Inventory(slots);
            var slotIndex = random.Next(0, high_size);

            var result = inventory.GetItem(slotIndex);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetItem_from_index__Wrong_index_should_return_null()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var inventory = new Inventory(slots);

            var slotIndex = -1;

            var result = inventory.GetItem(slotIndex);

            Assert.IsNull(result);
        }

        [Test]
        public void GetItem_from_empty_index__Should_return_null()
        {
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(), high_size).ToArray();

            var inventory = new Inventory(slots);

            var result = inventory.GetItem(random.Next(0,high_size));

            Assert.IsNull(result);
        }
        #endregion

        //TODO: RE-ADD WHEN IMPLEMENTS 

        /*
        #region GetItemAmount(Index)
        [Test]
        public void GetItemAmount_from_index__All_amount_Should_return_the_Items()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var slotIndex = random.Next(0, high_size);
            var inventory = new Inventory(slots);

            var randomCollectedAmount = random.Next(1, amount);
            var result = inventory.GetItemAmount(slotIndex, randomCollectedAmount);

            Assert.IsNotNull(result);
            Assert.AreEqual(randomCollectedAmount,result.Length);
        }

        [Test]
        public void GetItemAmount_from_index__Bigger_amount_should_return_the_max_Items()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var slotIndex = random.Next(0, high_size);
            var inventory = new Inventory(slots);

            var randomCollectedAmount = random.Next(high_amount, high_amount * 10);
            var result = inventory.GetItemAmount(slotIndex, randomCollectedAmount);

            Assert.IsNotNull(result);
            Assert.AreEqual(amount, result.Length);
        }

        [Test]
        public void GetItemAmount_from_index__Wrong_amount_should_return_empty()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var slotIndex = random.Next(0, high_size);
            var inventory = new Inventory(slots);

            var randomCollectedAmount = -1;
            var result = inventory.GetItemAmount(slotIndex, randomCollectedAmount);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }
        #endregion
        */

        #region GetItem(Item)
        [Test]
        public void GetItem__Should_return_Item()
        {
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false), high_size).ToArray();

            var item = this.DefaultItemGenerator();
            slots[random.Next(0, slots.Length)] = new StackSlot(item);

            var inventory = new Inventory(slots);

            var result = inventory.GetItem(item);

            Assert.AreEqual(item,result);
        }

        [Test]
        public void GetItem__Wrong_item_should_return_null()
        {
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(true), high_size).ToArray();
            var inventory = new Inventory(slots);

            var item = this.DistinctItemGenerator();
            var result = inventory.GetItem(item);

            Assert.IsNull(result);
            Assert.AreNotEqual(item, result);
        }

        [Test]
        public void GetItem__Null_item_should_return_null()
        {
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(true), high_size).ToArray();

            var inventory = new Inventory(slots);

            var result = inventory.GetItem(null);

            Assert.IsNull(result);
        }
        #endregion

        #region GetItemAmount(Item)
        [Test]
        public void GetItemAmount__Should_return_Item()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false), high_size).ToArray();

            var item = new Item(
              id: Guid.NewGuid().ToString() + "2",
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: high_amount // has amount changed
           );

            var randomCollectedAmount = random.Next(1, amount);
            slots[random.Next(0, slots.Length)] = new StackSlot(item, randomCollectedAmount);

            var inventory = new Inventory(slots);

            var result = inventory.GetItemAmount(item, randomCollectedAmount);

            Assert.AreEqual(randomCollectedAmount, result.Length);
        }

        [Test]
        public void GetItemAmount__Wrong_Item_should_return_empty()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var item = new Item(
              id: Guid.NewGuid().ToString() + "2",
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: high_amount
           );

            var inventory = new Inventory(slots);

            var randomCollectedAmount = random.Next(high_amount, high_amount * 10);
            var result = inventory.GetItemAmount(item, randomCollectedAmount);

            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void GetItemAmount__Bigger_amount_should_return_the_max_Items()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var item = new Item(
                id: Guid.NewGuid().ToString() + "2",
                name: Guid.NewGuid().ToString(),
                description: Guid.NewGuid().ToString(),
                image: null,
                maxStack: high_amount
            );

            var slotIndex = random.Next(0, high_size);
            slots[slotIndex] = new StackSlot(item, amount);

            var inventory = new Inventory(slots);

            var randomCollectedAmount = random.Next(high_amount, high_amount * 10);
            var result = inventory.GetItemAmount(item, randomCollectedAmount);

            Assert.IsNotNull(result);
            Assert.AreEqual(amount, result.Length);
        }

        [Test]
        public void GetItemAmount__Wrong_amount_should_return_empty()
        {
            var amount = random.Next(low_amount, high_amount);
            var slots = Enumerable.Repeat(this.DefaultSlotGenerator(false, amount), high_size).ToArray();

            var inventory = new Inventory(slots);

            var item = this.DistinctItemGenerator();
            var randomCollectedAmount = -1;

            var result = inventory.GetItemAmount(item, randomCollectedAmount);

            Assert.AreEqual(0, result.Length);
        }
        #endregion
    }
}
