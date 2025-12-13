namespace TheChest.Inventories.Tests.Containers
{
    //TODO: find a way to create a complex inventory for this test 
    // with multiple items in each slot
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void AddItemAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(item, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void AddItemAt_InvalidItem_ThrowsArgumentException()
        {
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(default(T)!, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void AddItemAt_EmptySlot_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index);

            Assert.That(inventory[index].Content, Has.One.EqualTo(item));
        }

        [Test]
        public void AddItemAt_EmptySlot_CallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.AddAt(item, index);
        }

        [Test]
        public void AddItemAt_EmptySlot_ReturnsTrue()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.True);
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void AddItemAt_SlotWithDifferentItem_DoNotAddItem()
        {
            var index = this.random.Next(0, 20);
            var items = this.itemFactory.CreateManyRandom(20);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 5, items);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index);

            Assert.That(inventory[index].Content, Has.No.AnyOf(item));
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_ReturnsFalse()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.FullContainer(20, 5, this.itemFactory.CreateRandom());

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.False);
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_DoesNotCallOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.FullContainer(20, 5, this.itemFactory.CreateRandom());

            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");
            inventory.AddAt(item, index);
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            inventory.Get(index);

            var stackSize = inventory[index].StackAmount;
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize + 1));
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_CallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.AddAt(item, index);
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_ReturnsTrue()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItemAt_FullSlotWithSameItem_DoesNotAddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            var item = this.itemFactory.CreateDefault();

            inventory.AddAt(item, index);

            Assert.That(inventory[index].Content, Has.No.AnyOf(item));
        }

        [Test]
        public void AddItemAt_FullSlotWithSameItem_DoNotCallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            var item = this.itemFactory.CreateDefault();

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");
            inventory.AddAt(item, index);
        }

        [Test]
        public void AddItemAt_FullSlotWithSameItem_ReturnsFalse()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.False);
        }
    }
}
