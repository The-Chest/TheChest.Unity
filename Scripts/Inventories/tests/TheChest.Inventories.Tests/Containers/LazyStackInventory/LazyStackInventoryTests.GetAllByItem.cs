namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void GetAll_ByItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.GetAll(item: default!));
        }

        [Test]
        public void GetAll_ByItem_ExistingItem_CallsOnGetEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 10);
            var invalidRandomItems = this.itemFactory.CreateManyRandom(inventorySize - 5);
            var validItems = this.itemFactory.CreateMany(inventorySize - invalidRandomItems.Length);
            var items = invalidRandomItems.Concat(validItems).ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, items);

            var item = this.itemFactory.CreateDefault();
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(validItems.Length));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.InRange(1, stackSize));
                });
            };
            inventory.GetAll(item);
        }

        [Test]
        public void GetAll_ByItem_ExistingItem_ReturnsAllMatchingItems()
        {
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventoryRandomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.ShuffledItemsContainer(10, 5, inventoryItem, inventoryRandomItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.GetAll(item);

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void GetAll_ByItem_ExistingItem_RemovesAllMatchingItemsFromInventory()
        {
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventoryRandomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.ShuffledItemsContainer(10, 5, inventoryItem, inventoryRandomItem);

            var item = this.itemFactory.CreateDefault();
            inventory.GetAll(item);

            Assert.That(inventory.Slots, Has.None.EqualTo(item));
        }

        [Test]
        public void GetAll_ByItem_DifferentItems_DoesNotCallOnGetEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 10);
            var invalidRandomItems = this.itemFactory.CreateManyRandom(inventorySize);
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, invalidRandomItems);

            var item = this.itemFactory.CreateDefault();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for an empty slot.");
            inventory.GetAll(item);
        }

        [Test]
        public void GetAll_ByItem_DifferentItems_ReturnsEmptyArray()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 10);
            var invalidRandomItems = this.itemFactory.CreateManyRandom(inventorySize);
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, invalidRandomItems);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.GetAll(item);

            Assert.That(result, Is.Empty, "Expected an empty array when no matching items are found.");
        }
    }
}
