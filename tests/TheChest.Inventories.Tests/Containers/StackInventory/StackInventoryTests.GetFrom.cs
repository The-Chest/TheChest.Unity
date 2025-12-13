namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void GetFrom_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(index), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetFrom_EmptySlot_ReturnsNull()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = inventory.Get(0);
            Assert.That(item, Is.Null);
        }

        [Test]
        public void GetFrom_EmptySlot_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            inventory.Get(0);
        }

        [Test]
        public void GetFrom_SlotWithItems_ReturnsItem()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var item = inventory.Get(index);

            Assert.That(item, Is.EqualTo(slotItem));
        }

        [Test]
        public void GetFrom_SlotWithItems_RemovesItemFromSlot()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            inventory.Get(index);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize - 1));
        }

        [Test]
        public void GetFrom_SlotWithItems_CallsOnGetEvent()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var index = this.random.Next(0, 20);
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.Get(index);
        }
    }
}
