namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void GetItems_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Get(item, amount),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void GetItems_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(item: default!, amount: 1), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItems_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.Get(this.itemFactory.CreateRandom(), 10);
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet should not be called if no item is found");
        }

        [Test]
        public void GetItems_ValidAmountNotFoundItem_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var amount = this.random.Next(1, size);
            var result = inventory.Get(this.itemFactory.CreateRandom(), amount);

            Assert.That(result, Has.Length.EqualTo(0));
        }

        [Test]
        public void GetItems_ValidAmountFullInventory_ReturnsItemArrayWithAmountSize()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var amount = this.random.Next(1, size);
            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(amount));
        }

        [Test]
        public void GetItems_ValidAmountWithItems_ReturnsItemArrayWithMaxAvailable()
        {
            var inventorySize = this.random.Next(10, 20);
            var expectedAmount = this.random.Next(1, inventorySize / 2);
            var items = this.itemFactory.CreateMany(expectedAmount);
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, items);

            var amount = this.random.Next(expectedAmount, inventorySize);
            var result = inventory.Get(items[0], amount);
        
            Assert.That(result, Has.Length.EqualTo(expectedAmount));
        }

        [Test]
        public void GetItems_ExistingItemOnSlot_CallsOnGetEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var expectedAmount = this.random.Next(1, inventorySize / 2);
            var items = this.itemFactory.CreateMany(expectedAmount);
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, items);

            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(expectedAmount));
                Assert.That(args.Data.Select(x => x.Item), Is.EqualTo(items));
            };
            inventory.Get(items[0], expectedAmount);
        }
    }
}
