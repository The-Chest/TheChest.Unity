namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void AddAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.Throws<ArgumentNullException>(() => inventory.AddAt(default!, 0, 1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddAt_ZeroOrNegativeAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.AddAt(item, 0, amount));
        }

        [TestCase(-1)]
        [TestCase(200)]
        public void AddAt_ThrowsArgumentOutOfRangeException_WhenIndexIsInvalid(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.AddAt(item, index, 1));
        }

        [Test]
        public void AddAt_AvailableSlot_AddsItemToSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex, stackSize);

            Assert.Multiple(() =>
            {
                var slot = inventory[randomIndex];
                Assert.That(slot.Content, Has.All.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.False);
                Assert.That(slot.StackAmount, Is.EqualTo(stackSize));
            });
        }

        [Test]
        public void AddAt_AllItemsSuccessfullyAdded_ReturnsZero()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var notAddedCount = inventory.AddAt(item, 0, stackSize);
            Assert.That(notAddedCount, Is.Zero);
        }

        [Test]
        public void AddAt_AllItemsSuccessfullyAdded_CallsOnAddEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var index = this.random.Next(0, size);
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                    Assert.That(firstEvent.Amount, Is.EqualTo(stackSize));
                });
            };
            inventory.AddAt(item, index, stackSize);
        }

        [Test]
        public void AddAt_AvailableSlot_AddsMaxPossibleAmountOfItemToTheSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            inventory.AddAt(item, randomIndex, amount);

            Assert.Multiple(() =>
            {
                var slot = inventory[randomIndex];
                Assert.That(slot.Content, Has.All.EqualTo(item));
                Assert.That(slot.IsFull, Is.True);
                Assert.That(slot.StackAmount, Is.EqualTo(stackSize));
            });
        }

        [Test]
        public void AddAt_NotAllItemsAdded_CallsOnAddEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;

            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(randomIndex));
                    Assert.That(firstEvent.Amount, Is.EqualTo(stackSize));
                });
            };
            inventory.AddAt(item, randomIndex, amount);
        }

        [Test]
        public void AddAt_NotAllItemsAdded_ReturnsNotAddedAmount()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            var notAddedCount = inventory.AddAt(item, randomIndex, amount);

            Assert.That(notAddedCount, Is.EqualTo(amount - stackSize));
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_DoesNotAddItems()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex, stackSize);

            Assert.That(inventory[randomIndex].Content, Has.None.EqualTo(item));
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when adding to a slot with a different item.");
            inventory.AddAt(item, randomIndex, stackSize);
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_ReturnsNotAddedAmount()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItem = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var result = inventory.AddAt(item, randomIndex, stackSize);

            Assert.That(result, Is.EqualTo(stackSize));
        }
    }
}
