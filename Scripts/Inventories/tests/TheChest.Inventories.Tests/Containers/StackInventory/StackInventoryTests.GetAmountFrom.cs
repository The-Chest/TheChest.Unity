namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void GetAmountFrom_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(index, 10), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetAmountFrom_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(0, amount), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAmountFrom_EmptySlot_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = inventory.Get(0, 10);
            Assert.That(item, Is.Empty);
        }

        [Test]
        public void GetAmountFrom_EmptySlot_CallsOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            inventory.Get(0, 10);
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_ReturnsItems()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(10, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var items = inventory.Get(index, 10);

            Assert.That(items, Is.Not.Empty);
            Assert.That(items.Count, Is.EqualTo(10));
            Assert.That(items, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_RemovesItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(10, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var removeAmount = this.random.Next(1, stackSize);
            inventory.Get(index, removeAmount);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize - removeAmount));
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_CallsOnGetEvent()
        {
            var stackSize = this.random.Next(10, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var index = this.random.Next(0, 20);
            var amount = this.random.Next(1, stackSize);
            inventory.OnGet += (sender, args) => {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(amount));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.Get(index, amount);
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_ReturnsTheMaximumAmountPossible()
        {
            var stackSize = this.random.Next(1, 10);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var amount = this.random.Next(stackSize + 1, stackSize * 2);
            var index = this.random.Next(0, 20);
            var items = inventory.Get(index, amount);

            Assert.That(items.Count, Is.EqualTo(stackSize));
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_RemovesAllItemsFromSlot()
        {
            var stackSize = this.random.Next(1, 10);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var index = this.random.Next(0, 20);
            var amount = this.random.Next(stackSize + 1, stackSize * 2);
            inventory.Get(index, amount);

            Assert.That(inventory[index].StackAmount, Is.Zero);
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_CallsOnGetEventWithTheMaximumAmountPossible()
        {
            var stackSize = this.random.Next(1, 10);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);
            
            var index = this.random.Next(0, 20);
            var amount = this.random.Next(stackSize + 1, stackSize * 2);
            inventory.OnGet += (sender, args) => {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
            };
            inventory.Get(index, amount);
        }
    }
}
