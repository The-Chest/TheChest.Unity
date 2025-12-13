namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryLazyStackSlotTests<T> 
    {
        [Test]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.EmptySlot();

            Assert.Throws<ArgumentNullException>(() => slot.Replace(default!, 1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1000)]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var slot = this.slotFactory.EmptySlot(10);
            var item = this.itemFactory.CreateDefault();

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Replace(item, amount));
        }

        [Test]
        public void Replace_EmptySlot_AddItems()
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();
            int amount = this.random.Next(1, 10);

            slot.Replace(item, amount);
            
            Assert.Multiple(() =>
            {
                Assert.That(slot.IsEmpty, Is.False);
                Assert.That(slot.StackAmount, Is.EqualTo(amount));
                Assert.That(slot.Content, Has.All.EqualTo(item));
            });
        }

        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();
            int amount = this.random.Next(1, 10);

            var result = slot.Replace(item, amount);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_SlotWithItems_ReplaceSContent()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(11, 20);
            int initialAmount = this.random.Next(1, 10);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var newItem = this.itemFactory.CreateRandom();
            int newAmount = this.random.Next(1, 10);
            var result = slot.Replace(newItem, newAmount);

            Assert.Multiple(() =>
            {
                Assert.That(slot.StackAmount, Is.EqualTo(newAmount));
                Assert.That(slot.Content, Has.All.EqualTo(newItem));
            });
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsItemFromSlot()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(11, 20);
            int initialAmount = this.random.Next(1, 10);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var newItem = this.itemFactory.CreateRandom();
            int newAmount = this.random.Next(1, 10);
            var result = slot.Replace(newItem, newAmount);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Length.EqualTo(initialAmount));
                Assert.That(result, Has.All.EqualTo(initialItem));
            });
        }
    }
}
