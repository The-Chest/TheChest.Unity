namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(1000)]
        public void GetAll_ByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.GetAll(index));
        }

        [Test]
        public void GetAll_ByIndex_ExistingIndex_ReturnsAllItemsFromSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, expectedItem);
            var index = this.random.Next(0, size);

            var result = inventory.GetAll(index);

            Assert.That(
                result, 
                Is.Not.Empty
                    .And.Length.EqualTo(stackSize)
                    .And.All.EqualTo(expectedItem)
            );
        }

        [Test]
        public void GetAll_ByIndex_ExistingIndex_RemovesAllItemsFromSlot()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, expectedItem);
            var index = this.random.Next(0, size);

            inventory.GetAll(index);
            
            Assert.Multiple(() =>
            {
                Assert.That(inventory[index].IsEmpty, Is.True);
                Assert.That(inventory[index].Content, Is.Empty);
            });
        }

        [Test]
        public void GetAll_ByIndex_ExistingIndex_CallsOnGetEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size);
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));

                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Item, Is.EqualTo(expectedItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                    Assert.That(firstEvent.Amount, Is.EqualTo(stackSize));
                });
            };
            inventory.GetAll(index);
        }

        [Test]
        public void GetAll_ByIndex_NotExistingIndex_ReturnsEmptyArray()
        {
            var size = this.random.Next(1, 20);
            var inventory = this.containerFactory.EmptyContainer(size);
            var index = this.random.Next(0, size);

            var result = inventory.GetAll(index);

            Assert.That(result,Is.Empty);
        }

        [Test]
        public void GetAll_ByIndex_NotExistingIndex_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(1, 20);
            var inventory = this.containerFactory.EmptyContainer(size);
            var index = this.random.Next(0, size);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for an empty slot.");
            inventory.GetAll(index);
        }
    }
}
