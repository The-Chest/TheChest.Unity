namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void CanReplace_NullItem_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();

            var result = slot.CanReplace(default!, 1);

            Assert.That(result, Is.False);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanReplace_InvalidAmount_ReturnsFalse(int amount)
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();

            var result = slot.CanReplace(item, amount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplace_AmountExceedingMaxStackAmount_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot(10);
            var item = this.itemFactory.CreateDefault();

            var result = slot.CanReplace(item, 11);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplace_EmptySlotValidItemAndAmount_ReturnsTrue()
        {
            var slot = this.slotFactory.EmptySlot(10);
            var item = this.itemFactory.CreateDefault();

            var result = slot.CanReplace(item, 5);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplace_FullSlotSlotValidItemAndAmount_ReturnsTrue()
        {
            var slot = this.slotFactory.FullSlot(this.itemFactory.CreateRandom(), 10);
            var item = this.itemFactory.CreateDefault();

            var result = slot.CanReplace(item, 5);

            Assert.That(result, Is.True);
        }
    }
}
