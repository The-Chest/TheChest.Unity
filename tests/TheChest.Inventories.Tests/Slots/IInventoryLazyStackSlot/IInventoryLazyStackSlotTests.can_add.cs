namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void CanAdd_NullItem_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();

            var result = slot.CanAdd(default!, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAdd_FullSlot_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item);

            var result = slot.CanAdd(item, 1);

            Assert.That(result, Is.False);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanAdd_InvalidAmount_ReturnsFalse(int amount)
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();

            var result = slot.CanAdd(item, amount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAdd_EmptySlotAndValidItem_ReturnsTrue()
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();

            var result = slot.CanAdd(item, 1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAdd_NotEmptySlotAndItemMatchesContent_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, 5, 10);

            var secondItem = this.itemFactory.CreateDefault();

            var result = slot.CanAdd(secondItem, 5);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAdd_NotEmptySlotAndItemDoesNotMatchContent_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, 5, 10);

            var secondItem = this.itemFactory.CreateRandom();

            var result = slot.CanAdd(secondItem, 5);

            Assert.That(result, Is.False);
        }
    }
}
