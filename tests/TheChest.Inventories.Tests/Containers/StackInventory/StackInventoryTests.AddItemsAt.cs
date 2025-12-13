namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void AddItemsAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(items, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void AddItemsAt_EmptySlot_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var items = this.itemFactory.CreateMany(10);
            inventory.AddAt(items, index);

            Assert.That(inventory[index].Content, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_EmptySlot_CallsOnAddEvent()
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
            inventory.AddAt(items, index);
        }

        [Test]
        public void AddItemsAt_EmptySlot_ReturnsEmpty()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_DoesNotCallOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.OnAdd += (sender, e) => Assert.Fail("OnAdd event should not be called when is not possible to Add.");
            inventory.AddAt(items, index);
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_ReturnsItemsFromParams()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_SlotWithSameItem_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(5, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index, 2); 

            var items = this.itemFactory.CreateMany(2);
            inventory.AddAt(items, index);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[index].IsFull, Is.True);
                Assert.That(inventory[index].StackAmount, Is.EqualTo(amount));
                Assert.That(inventory[index].Content?.Reverse().Take(2), Is.EqualTo(items));
            });
        }

        [Test]
        public void AddItemsAt_FullSlotSlotWithSameItem_ReturnsNotAddedItemsFromParam()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_DoNotAddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(10);
            inventory.AddAt(items, index);

            Assert.That(inventory[index].Content, Has.No.AnyOf(items));
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_DoesNotCallOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(10);
            inventory.OnAdd += (sender, e) => Assert.Fail("OnAdd event should not be called when is not possible to Add.");
            inventory.AddAt(items, index);
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_ReturnsNotAddedItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.Not.Empty.And.EqualTo(items));
        }
    }
}
