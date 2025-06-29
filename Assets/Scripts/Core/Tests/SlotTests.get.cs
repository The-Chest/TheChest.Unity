using NUnit.Framework;
using System;
using System.Linq;
using TheChest.Examples.Containers;
using TheChest.Examples.Items;

namespace TheWorld.Tests.TheChest
{
    public partial class SlotTests
    {
        #region GetOne
        [Test]
        public void GetOne__Should_return_a_Item()
        {
            var item = new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: 1
           );

            var slot = new StackSlot(item);

            var resultItem = slot.GetOne();

            Assert.AreEqual(item,resultItem);
            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.CurrentItem);
        }

        [Test]
        public void GetOne__Empty_Slot_should_return_null()
        {
            var slot = new StackSlot(null);

            var resultItem = slot.GetOne();

            Assert.IsNull(resultItem);
            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.CurrentItem);
        }

        #endregion

        #region GetAll
        [Test]
        public void GetAll__Should_return_All_Items()
        {
            var stackAmount = random.Next(2, high_amount);

            var item = new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: stackAmount
           );

            var slot = new StackSlot(item, stackAmount);

            var resultItem = slot.GetAll();

            Assert.AreEqual(stackAmount, resultItem.Length);
            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.CurrentItem);
        }

        [Test]
        public void GetAll_Should_return_Max_Items()
        {
            var stackAmount = random.Next(2, high_amount);

            var item = new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: stackAmount
            );

            var slot = new StackSlot(item, stackAmount);

            var result = slot.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(stackAmount, result.Length);
        }
        #endregion

        #region GetAmount
        [Test]
        public void GetAmount__should_return_the_especified_amount()
        {
            var randomAmount = random.Next(1, low_amount);
            var expectedAmount = random.Next(low_amount, high_amount);

            var item = this.DefaultItemGenerator();
            var slot = new StackSlot(item, expectedAmount);

            var result = slot.GetAmount(randomAmount);

            var resultAmount = result.Length;
            Assert.IsNotNull(result);
            Assert.AreEqual(randomAmount, resultAmount);
            Assert.AreEqual( (expectedAmount - resultAmount) == 0,slot.IsEmpty);
        }

        [Test]
        public void GetAmount__big_amount_should_return_the_max_capacity()
        {
            var searchedAmount = random.Next(low_amount, high_amount);
            var itemAmount = random.Next(1, low_amount);

            var item = this.DefaultItemGenerator();
            var slot = new StackSlot(item, itemAmount);

            var result = slot.GetAmount(searchedAmount);

            Assert.IsTrue(slot.IsEmpty);
            Assert.AreEqual(itemAmount,result.Length);
        }
        #endregion

        #region Replace
        [Test]
        public void Replace_empty_Slot__Should_return_empty_array()
        {
            //Arrange
            var item = this.DefaultItemGenerator();

            var slot = new StackSlot();
            
            //Act
            var result = slot.Replace(item,1);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void Replace__Should_return_old_obejct()
        {
            //Item to replace
            var newItem = this.DefaultItemGenerator();
            var newItemAmountRandom = random.Next(1, low_amount);

            //Item to be replaced
            var oldItem = this.DefaultItemGenerator();
            var oldItemAmountRandom = random.Next(1, low_amount);

            //Act
            var slot = new StackSlot(oldItem, oldItemAmountRandom);

            var results = slot.Replace(newItem, newItemAmountRandom);

            //Assert
            Assert.AreEqual(oldItemAmountRandom,results.Length);

            foreach (var result in results)
            {
                Assert.AreEqual(oldItem,result);
            }

            Assert.AreEqual(newItem,slot.CurrentItem);
            Assert.AreEqual(newItemAmountRandom,slot.StackAmount);
        }
    
        [Test]
        public void Replace__Negative_amount_should_not_Replace()
        {
            //Item to replace
            var newItem = this.DefaultItemGenerator();
            var newItemAmountRandom = random.Next(int.MinValue,-1);

            //Item to be replaced
            var oldItem = this.DefaultItemGenerator();
            var oldItemAmountRandom = random.Next(1, low_amount);

            var slot = new StackSlot(oldItem, oldItemAmountRandom);

            var results = slot.Replace(newItem, newItemAmountRandom);

            //Tests
            Assert.IsEmpty(results);
            Assert.AreEqual(slot.CurrentItem,oldItem);
            Assert.AreEqual(slot.StackAmount,oldItemAmountRandom);
        }

        [Test]
        public void Replace__Null_object_should_return_the_old_object()
        {
            Item newItem = null;

            var oldItem = this.DefaultItemGenerator();
            var oldItemAmountRandom = random.Next(1, low_amount);

            //Act
            var slot = new StackSlot(oldItem, oldItemAmountRandom);

            var results = slot.Replace(newItem);

            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.CurrentItem);
            //Assert.AreEqual(oldItemAmountRandom,results.Length);
        }

        [Test]
        public void Replace__Same_Item_Type_should_stack()
        {
            var itemAmount = random.Next(1, low_amount);
            var maxStack = itemAmount * 2 + random.Next(0, low_amount / 2);//to be possible to add twice and not get full

            var item = new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: maxStack
           );

            var slot = new StackSlot(item, itemAmount);

            var results = slot.Replace(item, itemAmount);

            Assert.Zero(results.Length);
            Assert.AreEqual(slot.StackAmount, itemAmount * 2);
        }

        [Test]
        public void Replace__Same_Item_Type_with_big_Amount__Should_stack_and_return_not_added_items()
        {
            var maxStack = random.Next(low_amount, high_amount);
            var itemAmount = maxStack / 2 + random.Next(1, low_amount / 2);//to be possible to add twice and overflow

            var item = new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: maxStack
           );

            var slot = new StackSlot(item, itemAmount);

            var results = slot.Replace(item, itemAmount);

            Assert.IsTrue(slot.IsFull);
            Assert.AreEqual(itemAmount * 2 - maxStack, results.Length);
        }
        #endregion

        #region Replace (Array)

        [Test]
        public void ReplaceArray_empty_Slot__Should_return_empty_array()
        {
            //Arrange
            var item = this.DefaultItemGenerator();
            var arraySize = random.Next(0, low_amount);

            var slot = new StackSlot();

            //Act
            var result = slot.Replace(Enumerable.Repeat(item, arraySize).ToArray());

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void ReplaceArray__Should_return_old_obejcts()
        {
            //Item to replace
            var newItem = this.DefaultItemGenerator();
            var newItemAmount = random.Next(1, low_amount);

            //Item to be replaced
            var oldItem = this.DefaultItemGenerator();
            var oldItemAmount = random.Next(1, low_amount);

            //Act
            var slot = new StackSlot(oldItem, oldItemAmount);

            var results = slot.Replace(Enumerable.Repeat(newItem, newItemAmount).ToArray());

            //Assert
            Assert.AreEqual(oldItemAmount, results.Length);

            foreach (var result in results)
            {
                Assert.AreEqual(oldItem, result);
            }

            Assert.AreEqual(newItem, slot.CurrentItem);
            Assert.AreEqual(newItemAmount, slot.StackAmount);
        }

        [Test]
        public void Replace__Null_array_should_return_old_object()
        {
            var oldItem = this.DefaultItemGenerator();
            var oldItemAmountRandom = random.Next(1, low_amount);

            //Act
            var slot = new StackSlot(oldItem, oldItemAmountRandom);

            var results = slot.Replace(null,1);

            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.CurrentItem);
            Assert.AreEqual(oldItemAmountRandom, results.Length);
        }

        [Test]
        public void Replace__Same_array_Type_should_stack()
        {
            var itemAmount = random.Next(1, low_amount);
            var maxStack = itemAmount * 2 + random.Next(0, low_amount / 2);//to be possible to add twice and not get full

            var item = new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: maxStack
            );

            var array = Enumerable.Repeat(item, itemAmount).ToArray();

            var slot = new StackSlot(item, itemAmount);

            var results = slot.Replace(array);

            Assert.Zero(results.Length);
            Assert.AreEqual(slot.StackAmount, itemAmount * 2);
        }

        [Test]
        public void Replace__Same_array_Type_with_big_Amount__Should_stack_and_return_not_added_items()
        {
            var maxStack = random.Next(low_amount, high_amount);
            var itemAmount = maxStack / 2 + random.Next(1, low_amount / 2);//to be possible to add twice and overflow

            var item = new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: maxStack
           );

            var array = Enumerable.Repeat(item, itemAmount).ToArray();

            var slot = new StackSlot(item, itemAmount);

            var results = slot.Replace(array);

            Assert.IsTrue(slot.IsFull);
            Assert.AreEqual(itemAmount * 2 - maxStack, results.Length);
        }

        #endregion
    }
}
