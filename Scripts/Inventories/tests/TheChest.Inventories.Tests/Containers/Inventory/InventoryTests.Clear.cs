namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var result = inventory.Clear();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet should not be called if no item is found");
            inventory.Clear();
        }

        [Test]
        public void Clear_FullInventory_ReturnsEveryItemFromInventory()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateManyRandom(size / 2).Concat(
                this.itemFactory.CreateMany(size / 2)
            ).ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);
            var result = inventory.Clear();
            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void Clear_FullInventory_CallsOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateManyRandom(size / 2).Concat(
                this.itemFactory.CreateMany(size / 2)
            ).ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(items.Length));
                Assert.That(args.Data.Select(x => x.Item), Is.EquivalentTo(items));
            };
            inventory.Clear();
        }
    }
}
