namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void Get_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var slot = this.slotFactory.EmptySlot();

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Get(amount));
        }

        [Test]
        public void Get_EmptySlot_ReturnsEmptyArray()
        {
            var slot = this.slotFactory.EmptySlot();

            var result = slot.Get(1);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Get_AmountExceedingStackAmount_ReturnsAllItems()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var result = slot.Get(amount + 1);

            Assert.That(result, Has.Length.EqualTo(amount));
        }

        [Test]
        public void Get_AmountExceedingStackAmount_ClearsSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            slot.Get(amount + 1);

            Assert.That(slot.IsEmpty, Is.True);
        }

        [Test]
        public void Get_ValidAmount_ReturnsSpecifiedAmountOfItems()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var result = slot.Get(amount - 1);

            Assert.That(result, Has.Length.EqualTo(amount - 1));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void Get_ValidAmount_ReducesStackAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var getAmount = this.random.Next(1, amount);
            slot.Get(getAmount);

            Assert.That(slot.StackAmount, Is.EqualTo(amount - getAmount));
        }
    }
}
