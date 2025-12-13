namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void AddAt_WithReplace_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.Throws<ArgumentNullException>(
                () => inventory.AddAt(default!, 0, 1, false)
            ).Message.Contains("item");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddAt_WithReplace_ZeroOrNegativeAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(
                () => inventory.AddAt(item, 0, amount, false)
            ).Message.Contains("amount");
        }

        [TestCase(-1)]
        [TestCase(200)]
        public void AddAt_WithReplace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(
                () => inventory.AddAt(item, index, 1, false)
            ).Message.Contains("index");
        }

        [Test]
        public void AddAt_WithReplace_SuccessfullAdded_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            var result = inventory.AddAt(item, 0, 2, false);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddAt_WithReplace_AvailableSlot_AddsItemToSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex, stackSize, false);

            Assert.Multiple(() =>
            {
                var slot = inventory[randomIndex];
                Assert.That(slot.Content, Has.All.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.False);
                Assert.That(slot.StackAmount, Is.EqualTo(stackSize));
            });
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_SuccessfullAdded_ReturnsEmptyArray()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var result = inventory.AddAt(item, randomIndex, stackSize, true);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_AvailableSlot_AddsItemToSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex, stackSize, true);

            Assert.Multiple(() =>
            {
                var slot = inventory[randomIndex];
                Assert.That(slot.Content, Has.All.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.False);
                Assert.That(slot.StackAmount, Is.EqualTo(stackSize));
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAt_WithReplace_AvailableSlot_AddsMaxPossibleAmountOfItemToTheSlot(bool replace)
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            inventory.AddAt(item, randomIndex, amount, replace);

            Assert.Multiple(() =>
            {
                var slot = inventory[randomIndex];
                Assert.That(slot.Content, Has.All.EqualTo(item));
                Assert.That(slot.IsFull, Is.True);
                Assert.That(slot.StackAmount, Is.EqualTo(stackSize));
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAt_WithReplace_NotAllItemsAdded_ReturnsNotAddedAmount(bool replace)
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            var notAddedItems = inventory.AddAt(item, randomIndex, amount, replace);

            Assert.That(notAddedItems, Has.Length.EqualTo(amount - stackSize).And.All.EqualTo(item));
        }

        [Test]
        public void AddAt_WithReplace_ReplaceDisabled_WithItem_ReturnsNotAddedItems()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var amount = stackSize;
            var notAddedItems = inventory.AddAt(item, randomIndex, amount, false);

            Assert.That(notAddedItems, Has.Length.EqualTo(amount).And.All.EqualTo(item));
        }

        [Test]
        public void AddAt_WithReplace_ReplaceDisabled_WithItem_DoesNotAddToTheSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = stackSize;
            inventory.AddAt(item, index, amount, false);

            Assert.That(inventory[index].Content, Has.None.EqualTo(item).And.Length.EqualTo(amount));
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_WithDifferentItem_ReplacesItem()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = stackSize;
            inventory.AddAt(item, index, amount, true);

            Assert.That(inventory[index].Content, Has.Count.EqualTo(stackSize).And.All.EqualTo(item));
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_WithDifferentItem_ReturnsOldItemFromSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = stackSize;
            var oldItems = inventory.AddAt(item, index, amount, true);
        
            Assert.That(oldItems, Has.All.EqualTo(randomItems[index]));
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_WithDifferentItem_AmountBiggerThanSlotMaxAmount_DoesNotAddItem()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItem = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1,10) + stackSize;
            inventory.AddAt(item, index, amount, true);

            Assert.That(
                inventory[index].Content, 
                Has.None.EqualTo(item)    
            );
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_WithDifferentItem_AmountBiggerThanSlotMaxAmount_ReturnsNotAddedItems()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItem = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            var notAddedItems = inventory.AddAt(item, index, amount, true);

            Assert.That(notAddedItems, Has.Length.EqualTo(amount).And.All.EqualTo(item));
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_WithSameItem_AddsItem()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(11, 20);
            var randomItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, randomItem);

            var expectedAmountNotAdded = this.random.Next(1, 10);
            var index = this.random.Next(0, size);
            inventory.Get(index, stackSize - expectedAmountNotAdded);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize;
            inventory.AddAt(item, index, amount, true);

            Assert.That(inventory[index].Content, Has.Count.EqualTo(stackSize).And.All.EqualTo(item));
        }

        [Test]
        public void AddAt_WithReplace_ReplaceEnabled_WithSameItem_ReturnsNotAddedItems()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(11, 20);
            var randomItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, randomItem);

            var expectedAmountNotAdded = this.random.Next(1, 10);
            var index = this.random.Next(0, size);
            inventory.Get(index, stackSize - expectedAmountNotAdded);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize;
            var notAddedItems = inventory.AddAt(item, index, amount, true);
        
            Assert.That(notAddedItems, Has.Length.EqualTo(expectedAmountNotAdded).And.All.EqualTo(item));
        }
    }
}
