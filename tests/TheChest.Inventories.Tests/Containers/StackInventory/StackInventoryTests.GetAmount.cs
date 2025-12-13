namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void GetAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(item, amount), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAmount_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(default(T)!, 10), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetAmount_EmptyInventory_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.EmptyContainer();
            var amount = inventory.Get(item, 10);
            Assert.That(amount, Is.Empty);
        }

        [Test]
        public void GetAmount_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            inventory.Get(item, 10);
        }

        [Test]
        public void GetAmount_InventoryWithItems_ReturnsSearchedItems()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(inventorySize / 2)
                .Append(item)
                .Append(item)
                .ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);

            var items = inventory.Get(item, stackSize);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(item));
            });
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromSlot()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);

            inventory.Get(slotItem, stackSize);

            Assert.That(inventory[0].IsEmpty, Is.True);
        }

        [Test]
        public void GetAmount_InventoryWithItems_CallsOnGetEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.OnGet += (sender, args) => {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
            };  
            inventory.Get(slotItem, stackSize);
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromMultipleSlotsInOrder()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(3, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);

            inventory.Get(slotItem, stackSize + (stackSize - 2));
            
            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].StackAmount, Is.EqualTo(0));
                Assert.That(inventory[1].StackAmount, Is.EqualTo(2));
            });
        }

        [Test]
        public void GetAmount_InventoryWithItems_CallsOnGetEventFromMultipleSlotsInOrder()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(3, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(secondEvent.Items, Has.Length.EqualTo(stackSize - 2));
                    Assert.That(secondEvent.Index, Is.EqualTo(1));
                });
            };

            inventory.Get(slotItem, stackSize + (stackSize - 2));
        }
    }
}
