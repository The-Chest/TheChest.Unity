namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetAllFrom_EmptySlot_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var items = inventory.GetAll(0);
            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetAllFrom_EmptySlot_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty slot");
            inventory.GetAll(0);
        }

        [TestCase(-1)]
        [TestCase(100)]
        public void GetAllFrom_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.GetAll(index), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAllFrom_SlotWithItems_ReturnItems()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var items = inventory.GetAll(index);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(slotItem));
            });
        }

        [Test]
        public void GetAllFrom_SlotWithItems_RemovesAllItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            inventory.GetAll(index);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[index].IsEmpty, Is.True);
                Assert.That(inventory[index].Content, Is.Empty);
            });
        }

        [Test]
        public void GetAllFrom_SlotWithItems_CallsOnGetEvent()
        {
            var index = this.random.Next(0, 20);
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
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };

            inventory.GetAll(index);
        }
    }
}
