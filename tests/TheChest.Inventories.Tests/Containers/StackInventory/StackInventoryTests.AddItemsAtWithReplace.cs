namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1, true)]
        [TestCase(-1, false)]
        [TestCase(100, true)]
        [TestCase(100, false)]
        public void AddItemsWithReplaceAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, bool replace)
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(items, index, replace), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(false)]
        [TestCase(true)]
        public void AddItemsWithReplaceAt_InvalidItem_ThrowsArgumentException(bool replace)
        {
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(Array.Empty<T>(), 0, replace), Throws.ArgumentException);
        }

        [Test]
        public void AddItemsWithReplaceAt_EmptySlot_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var items = this.itemFactory.CreateMany(10);
            inventory.AddAt(items, index, replace: true);

            Assert.That(inventory[index].Content, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsWithReplaceAt_EmptySlot_ReplaceEnabled_CallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var items = this.itemFactory.CreateMany(10);
            inventory.OnAdd += (sender, e) => {
                Assert.That(e.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.AddAt(items, index, replace: true);
        }

        [Test]
        public void AddItemsWithReplaceAt_EmptySlot_ReplaceEnabled_ReturnsEmpty()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index, replace: true);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithDifferentItem_ReplaceDisabled_DoNotReplaceItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.AddAt(items, index, replace: false);

            Assert.That(inventory[index].Content, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithDifferentItem_ReplaceDisabled_DoesNotCallOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.OnAdd += (sender, e) => Assert.Fail("OnAdd event should not be called when is not possible to Add or Replace.");
            inventory.AddAt(items, index, replace: false);
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithDifferentItem_ReplaceDisabled_ReturnsItemsFromParams()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index, replace: false);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithDifferentItem_ReplaceEnabled_ReturnsItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index, replace: true);

            Assert.That(result, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithDifferentItems_ReplaceEnabled_ReplacesItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.AddAt(items, index, replace: true);

            Assert.That(inventory[index].Content, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithDifferentItems_ReplaceEnabled_CallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.OnAdd += (sender, e) => {
                Assert.That(e.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.AddAt(items, index, replace: true);
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithSameItem_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(5, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index, 2);

            var items = this.itemFactory.CreateMany(2);
            inventory.AddAt(items, index, replace: true);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[index].IsFull, Is.True);
                Assert.That(inventory[index].StackAmount, Is.EqualTo(amount));
                Assert.That(inventory[index].Content?.Reverse().Take(2), Is.EqualTo(items));
            });
        }

        [Test]
        public void AddItemsWithReplaceAt_FullSlotSlotWithSameItem_ReplaceEnabled_ReturnsParamItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index, replace: true);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithSameItem_ReplaceDisabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(5, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index, 2);

            var items = this.itemFactory.CreateMany(2);
            inventory.AddAt(items, index, replace: false);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[index].IsFull, Is.True);
                Assert.That(inventory[index].StackAmount, Is.EqualTo(amount));
                Assert.That(inventory[index].Content, Is.Not.Null);
                Assert.That(inventory[index].Content!.Reverse().Take(2), Is.EqualTo(items));
            });
        }

        [Test]
        public void AddItemsWithReplaceAt_SlotWithSameItem_ReplaceDisabled_CallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(5, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index, 2);

            var items = this.itemFactory.CreateMany(2);
            inventory.OnAdd += (sender, e) => {
                Assert.That(e.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.AddAt(items, index, replace: false);
        }

        [Test]
        public void AddItemsWithReplaceAt_FullSlotWithSameItem_ReplaceDisabled_ReturnsParamItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index, replace: false);

            Assert.That(result, Is.EqualTo(items));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemsWithReplaceAt_FullSlotWithSameItem_DoNotAddsToStack(bool replace)
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(10);
            inventory.AddAt(items, index, replace);

            Assert.That(inventory[index].Content, Has.No.AnyOf(items));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemsWithReplaceAt_FullSlotWithSameItem_DoesNotCallOnAddEvent(bool replace)
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(10);
            inventory.OnAdd += (sender, e) => Assert.Fail("OnAdd event should not be called when is not possible to Add or Replace.");
            inventory.AddAt(items, index, replace);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemsWithReplaceAt_FullSlotWithSameItem_ReturnsNotAddedItems(bool replace)
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index, replace);

            Assert.That(result, Is.Not.Empty.And.EqualTo(items));
        }
    }
}