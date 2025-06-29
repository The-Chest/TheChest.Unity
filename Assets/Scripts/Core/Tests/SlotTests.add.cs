using System;
using NUnit.Framework;
using System.Linq;
using TheChest.Examples.Containers;
using TheChest.Examples.Items;

namespace TheWorld.Tests.TheChest
{
    public partial class SlotTests
    {
        #region Add 
        [Test]
        public void Add__Should_add_Item_to_empty_Slot()
        {
            var item = this.DefaultItemGenerator();

            var slot = new StackSlot();

            var result = slot.Add(item);

            Assert.IsTrue(result);//should IsTrue
            Assert.IsFalse(slot.IsEmpty);//Should not be empty
            Assert.IsNotNull(slot.CurrentItem);//Should not be null
            Assert.AreSame(item, slot.CurrentItem);//Should be the item created
        }

        [Test]
        public void Add__Should_stack_Item_to_Slot()
        {
            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: 2
            );

            var slot = new StackSlot(item);

            var result = slot.Add(item);

            Assert.IsTrue(result);//Should be true
            Assert.IsTrue(slot.IsFull);//The inventory needs to be full

            Assert.AreEqual(item, slot.CurrentItem);//Should keep the currentItem
        }

        [Test]
        public void Add__Full_Slot_should_not_Add()
        {
            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: 1
           );

            var slot = new StackSlot(item);

            var result = slot.Add(item);

            Assert.IsFalse(result);//Should be false because the inventory is full
            Assert.IsTrue(slot.IsFull);//The inventory needs to be full
        }

        [Test]
        public void Add__Slot_with_other_Item_Type__Should_not_add()
        {
            var item = this.DefaultItemGenerator();
            var otherItem = this.DefaultItemGenerator();

            var slot = new StackSlot(otherItem);

            var result = slot.Add(item);

            Assert.IsFalse(result);//Should be false because cannot replace items
            Assert.AreEqual(otherItem, slot.CurrentItem);//Should keep the currentItem
        }
        #endregion

        #region AddAmount
        [Test]
        public void AddAmount__Should_add_the_amount_of_Items()
        {
            var amountAndStack = random.Next(1, high_amount);

            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: amountAndStack
            );

            var slot = new StackSlot();

            var result = slot.Add(item, amountAndStack);

            Assert.AreEqual(0, result);//Should be true
           
            Assert.IsTrue(slot.IsFull);//Verify if the inventory is full
        }

        [Test]
        public void AddAmount__Should_return_the_amount_that_coud_not_be_added()
        {
            var amount = random.Next(low_amount, high_amount);
            var maxStack = random.Next(1, low_amount - 1);

            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: maxStack
            );

            var slot = new StackSlot();

            var result = slot.Add(item, amount);

            Assert.AreEqual(Math.Abs(maxStack-amount), result);//Should be true
            Assert.IsTrue(slot.IsFull);
        }

        [Test]
        public void AddAmount__Should_stack_items()
        {
            var amount = random.Next(1, low_amount);
            var maxStack = amount * 2 + random.Next(0, low_amount / 2);

            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: maxStack
            );

            var slot = new StackSlot(item, amount);

            var result = slot.Add(item, amount);

            Assert.Zero(result);
            Assert.AreEqual(slot.StackAmount, amount * 2);
        }
        #endregion

        #region Add (Array)
        [Test]
        public void AddArray__Should_add_the_amount_of_Items()
        {
            var amountAndStack = random.Next(1, high_amount);

            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: amountAndStack
            );

            var arr = Enumerable.Repeat(item, amountAndStack).ToArray();

            var slot = new StackSlot();

            var result = slot.Add(arr);

            Assert.AreEqual(0, result);//Should be true

            Assert.IsTrue(slot.IsFull);//Verify if the inventory is full
        }

        [Test]
        public void AddArray__Should_return_the_amount_that_coud_not_be_added()
        {
            var amount = random.Next(low_amount, high_amount);
            var maxStack = random.Next(1, low_amount - 1);

            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: maxStack
            );

            var arr = Enumerable.Repeat(item, amount).ToArray();

            var slot = new StackSlot();

            var result = slot.Add(arr);

            Assert.AreEqual(Math.Abs(maxStack - amount), result);//Should be true
            Assert.IsTrue(slot.IsFull);
        }

        [Test]
        public void AddArray__Should_stack_items()
        {
            var amount = random.Next(1, low_amount);
            var maxStack = amount * 2 + random.Next(0, low_amount / 2);

            var item = new Item(
               id: Guid.NewGuid().ToString(),
               name: Guid.NewGuid().ToString(),
               description: Guid.NewGuid().ToString(),
               image: null,
               maxStack: maxStack
            );

            var arr = Enumerable.Repeat(item, amount).ToArray();

            var slot = new StackSlot(item, amount);

            var result = slot.Add(arr);

            Assert.Zero(result);
            Assert.AreEqual(slot.StackAmount, amount * 2);
        }
        #endregion
    }
}
