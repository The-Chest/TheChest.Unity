namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void GetCount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.GetCount(default!));
        }

        [Test]
        public void GetCount_NotFoundItem_ReturnsZero()
        {
            var wrongItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 10, wrongItem);

            var item = this.itemFactory.CreateDefault();
            var count = inventory.GetCount(item);

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void GetCount_ExistingItem_ReturnsCorrectCount()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var count = size * stackSize;
            var result = inventory.GetCount(item);

            Assert.That(result, Is.EqualTo(count));
        }
    }
}
