namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void Add_ItemIsNull_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.EmptySlot();

            Assert.Throws<ArgumentNullException>(() => slot.Add(default!, 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Add_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Add(item, amount));
        }

        [Test]
        public void Add_FullSlot_ReturnsNotAddedAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item);
            var amount = this.random.Next(1, 10);

            var result = slot.Add(item, amount);


            Assert.That(result, Is.EqualTo(amount));
        }

        [Test]
        public void Add_NotEmptySlotAndDifferentItem_DoNotAdd()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var maxAmount = this.random.Next(11, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var newItem = this.itemFactory.CreateRandom();
            slot.Add(newItem, 1);

            Assert.That(slot.Content, Has.No.AnyOf(newItem));
        }

        [Test]
        public void Add_NotEmptySlotAndDifferentItem_ReturnsNotAddedAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var maxAmount = this.random.Next(11, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var newItem = this.itemFactory.CreateRandom();
            
            var result = slot.Add(newItem, 1);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Add_EmptySlot_AddsItems()
        {
            var slot = this.slotFactory.EmptySlot();

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var result = slot.Add(item, amount);
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(0));
                Assert.That(slot.IsEmpty, Is.False);
            });
        }

        [Test]
        public void Add_SlotContainingSameItem_ReturnsNotAddedAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var maxAmount = this.random.Next(11, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var newAmount = this.random.Next(1, maxAmount - amount + 1);
            var result = slot.Add(item, newAmount);

            var expectedAmount = Math.Max(0, amount + newAmount - maxAmount);
            Assert.That(result, Is.EqualTo(expectedAmount));
        }

        [Test]
        public void Add_SlotContainingSameItem_AddItems()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var maxAmount = this.random.Next(11, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var newAmount = this.random.Next(1, maxAmount - amount);
            var newItem = this.itemFactory.CreateDefault();
            slot.Add(newItem, newAmount);

            Assert.That(slot.StackAmount, Is.EqualTo(amount + newAmount));
        }
    }
}
