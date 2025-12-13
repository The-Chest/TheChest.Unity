namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.Throws<ArgumentNullException>(() => inventory.Get(item: default!));
        }

        [Test]
        public void Get_ByItem_ExistingItem_ReturnsItem()
        {
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.ShuffledItemsContainer(10,5, items);
            var expectedItem = this.itemFactory.CreateDefault();

            var result = inventory.Get(expectedItem);

            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        public void Get_ByItem_ExistingItem_RemovesOneFromFirstFoundSlot()
        {
            var items = this.itemFactory.CreateDefault();
            var amount = 5;
            var inventory = this.containerFactory.FullContainer(10, amount, items);
            var expectedItem = this.itemFactory.CreateDefault();

            inventory.Get(expectedItem);

            Assert.That(inventory[0].StackAmount, Is.EqualTo(amount - 1));
        }

        [Test]
        public void Get_ByItem_ExistingItem_CallsOnGetEvent()
        {
            var items = this.itemFactory.CreateDefault();
            var amount = 5;
            var inventory = this.containerFactory.FullContainer(10, amount, items);
            var expectedItem = this.itemFactory.CreateDefault();

            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                var firstEvent = args.Data.First();
                Assert.Multiple(() =>
                {
                    Assert.That(firstEvent.Item, Is.EqualTo(expectedItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                    Assert.That(firstEvent.Amount, Is.EqualTo(1));
                });
            };
            inventory.Get(expectedItem);
        }

        [Test]
        public void Get_ByItem_NotFoundItem_ReturnsNull()
        {
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.ShuffledItemsContainer(10, 5, items);
            var item = this.itemFactory.CreateRandom();

            var result = inventory.Get(item);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_ByItem_NotFoundItem_DoesNotCallOnGetEvent()
        {
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.ShuffledItemsContainer(10, 5, items);
            var item = this.itemFactory.CreateRandom();

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when an item that is not found.");
            inventory.Get(item);
        }
    }
}
