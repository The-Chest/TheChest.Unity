namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetItem_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.Get(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItem_EmptyInventory_ReturnsNull()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer();

            var foundItem = inventory.Get(item);
            
            Assert.That(foundItem, Is.Null);
        }

        [Test]
        public void GetItem_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            var item = this.itemFactory.CreateDefault();
            inventory.Get(item);
        }

        [Test]
        public void GetItem_InventoryWithItems_ReturnsItem()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var item = inventory.Get(slotItem);

            Assert.That(item, Is.EqualTo(slotItem));
        }

        [Test]
        public void GetItem_InventoryWithItems_RemovesItemFromFirstFoundSlot()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            inventory.Get(slotItem);

            Assert.That(inventory[0].StackAmount, Is.EqualTo(stackSize - 1));
        }

        [Test]
        public void GetItem_InventoryWithItems_CallsOnGetEvent()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
            };
            inventory.Get(slotItem);
        }

        [Test]
        public void GetItem_InventoryWithDifferentItems_ReturnsNull()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Get(item);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetItem_InventoryWithDifferentItems_DoesNotCallOnGetEvent()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            var item = this.itemFactory.CreateDefault();
            inventory.Get(item);
        }
    }
}
