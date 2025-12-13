namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void GetItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItem_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.Get(this.itemFactory.CreateRandom());
            inventory.OnGet += (sender, args) => Assert.Fail("Get(T item) should not be called if no item is found");
        }

        [Test]
        public void GetItem_NoItems_ReturnsNull()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size / 2);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);
            
            var searchItem = this.itemFactory.CreateRandom();
            var result = inventory.Get(searchItem);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetItem_ExistingItems_ReturnsTheFirstFoundItem()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var result = inventory.Get(item);

            Assert.That(result, Is.Not.Null.And.EqualTo(item));
        }

        [Test]
        public void GetItem_ExistingItems_RemovesTheFirstFoundItemFromSlot()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            inventory.Get(item);

            Assert.That(inventory[0].Content, Is.Null);
        }

        [Test]
        public void GetItem_ExistingItems_CallsOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size / 2);
            var sameItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items.Concat(sameItems).ToArray());

            var randomItem = sameItems[0];
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(randomItem));
            };
            inventory.Get(randomItem);
        }
    }
}
