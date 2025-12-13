namespace TheChest.Inventories.Tests.Slots
{
    public abstract partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void GetAll_EmptySlot_ReturnsEmptyArray()
        {
            var slot = this.slotFactory.EmptySlot();
            var result = slot.GetAll();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetAll_SlotWithContent_ReturnsAllItemsAndClearSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var result = slot.GetAll();

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void GetAll_SlotWithContent_ClearsSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            slot.GetAll();
            
            Assert.Multiple(() =>
            {
                Assert.That(slot.Content, Is.Empty);
                Assert.That(slot.IsEmpty, Is.True);
            });
        }
    }
}
