namespace TheChest.Inventories.Tests.Containers
{
    //TODO: find a way to create a complex inventory for this test 
    // with multiple items in each slot
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1,true)]
        [TestCase(-1,false)]
        [TestCase(100, true)]
        [TestCase(100, false)]
        public void AddItemWithReplaceAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, bool replace)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(item, index, replace), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemWithReplaceAt_InvalidItem_ThrowsArgumentException(bool replace)
        {
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(default(T)!, 0, replace), Throws.ArgumentNullException);
        }

        [Test]
        public void AddItemWithReplaceAt_EmptySlot_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: true);

            Assert.That(inventory[index].Content, Has.One.EqualTo(item));
        }

        [Test]
        public void AddItemWithReplaceAt_EmptySlot_ReplaceEnabled_CallsOnAddEvent()
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
            inventory.AddAt(item, index, replace: true);
        }

        [Test]
        public void AddItemWithReplaceAt_EmptySlot_ReplaceEnabled_ReturnsEmpty()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: true);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void AddItemWithReplaceAt_SlotWithDifferentItem_ReplaceDisabled_DoNotReplaceItem()
        {
            var index = this.random.Next(0, 20);
            var items = this.itemFactory.CreateManyRandom(20);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 5, items);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: false);

            Assert.That(inventory[index].Content, Has.No.AnyOf(item));
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithDifferentItem_ReplaceDisabled_ReturnsItem()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.FullContainer(20, 5, this.itemFactory.CreateRandom());

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: false);

            Assert.That(result, Is.Not.Empty.And.Contains(item));
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithDifferentItem_ReplaceDisabled_DoesNotCallOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.FullContainer(20, 5, this.itemFactory.CreateRandom());

            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");
            inventory.AddAt(item, index, replace: false);
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithDifferentItem_ReplaceEnabled_ReturnsItemFromSlot()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: true);

            Assert.That(result, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithDifferentItem_ReplaceEnabled_CallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

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
            inventory.AddAt(item, index, replace: true);
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void AddItemWithReplaceAt_SlotWithDifferentItem_ReplaceEnabled_ReplacesItem()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: true);

            Assert.That(inventory[index].Content, Has.One.AnyOf(item));
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithSameItem_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            inventory.Get(index);

            var stackSize = inventory[index].StackAmount;
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: true);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize + 1));
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithSameItem_ReplaceEnabled_CallsOnAddEvent()
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
            inventory.AddAt(item, index, replace: true);
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithSameItem_ReplaceEnabled_ReturnsEmptyArray()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: true);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithSameItem_ReplaceDisabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            inventory.Get(index);

            var stackSize = inventory[index].StackAmount;
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: false);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize + 1));
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithSameItem_ReplaceDisabled_CallsOnAddEvent()
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
            inventory.AddAt(item, index, replace: false);
        }

        [Test]
        public void AddItemWithReplaceAt_SlotWithSameItem_ReplaceDisabled_ReturnsEmptyArray()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: false);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItemWithReplaceAt_FullSlotWithSameItem_DoNotAddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            var item = this.itemFactory.CreateDefault();

            inventory.AddAt(item, index, false);

            Assert.That(inventory[index].Content, Has.No.AnyOf(item));
        }

        [Test]
        public void AddItemWithReplaceAt_FullSlotWithSameItem_DoNotCallsOnAddEvent()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);
            var item = this.itemFactory.CreateDefault();

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");
            inventory.AddAt(item, index, false);
        }

        [Test]
        public void AddItemWithReplaceAt_FullSlotWithSameItem_ReturnsNotAddedItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, false);

            Assert.That(result, Is.Not.Empty.And.Contains(item));
        }
    }
}
