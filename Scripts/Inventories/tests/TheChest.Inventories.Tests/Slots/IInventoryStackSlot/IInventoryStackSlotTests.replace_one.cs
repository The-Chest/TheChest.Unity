namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void ReplaceOne_NullItemReplace_ThrowsArgumentNullException()
        {
            var slotItems = this.itemFactory.CreateMany(10);
            var slot = this.slotFactory.FullSlot(slotItems);

            Assert.That(() => slot.Replace(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void ReplaceOne_EmptySlot_AddsItem()
        {
            var slot = this.slotFactory.EmptySlot();

            var item = this.itemFactory.CreateDefault();
            var expectedResult = new T[1];
            Array.Fill(expectedResult, item);
            slot.Replace(item);

            Assert.That(slot.Content, Has.Count.EqualTo(1).And.EqualTo(expectedResult));
        }

        [Test]
        public void ReplaceOne_EmptySlot_ReturnsEmptyArray()
        {
            var slot = this.slotFactory.EmptySlot();

            var item = this.itemFactory.CreateDefault();
            var result = slot.Replace(item);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ReplaceOne_ItemDifferentFromSlot_ReturnsItemsFromSlot()
        {
            var items = this.itemFactory.CreateMany(5);
            var slot = this.slotFactory.WithItems(items);

            var replacingItem = this.itemFactory.CreateRandom();
            var result = slot.Replace(replacingItem);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void ReplaceOne_ItemDifferentFromSlot_AddsItemsToSlot()
        {
            var items = this.itemFactory.CreateMany(5);
            var slot = this.slotFactory.WithItems(items);

            var replacingItem = this.itemFactory.CreateRandom();
            var expectedResult = new T[1];
            Array.Fill(expectedResult, replacingItem);

            slot.Replace(replacingItem);

            Assert.That(slot.Content, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReplaceOne_ItemEqualToSlot_ReturnsItemsFromSlot()
        {
            var items = this.itemFactory.CreateMany(5);
            var slot = this.slotFactory.WithItems(items);

            var replacingItem = this.itemFactory.CreateDefault();
            var result = slot.Replace(replacingItem);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void ReplaceOne_ItemEqualToSlot_AddsItemsToSlot()
        {
            var items = this.itemFactory.CreateMany(5);
            var slot = this.slotFactory.WithItems(items);

            var replacingItem = this.itemFactory.CreateDefault();
            var expectedResult = new T[1];
            Array.Fill(expectedResult, replacingItem);

            slot.Replace(replacingItem);

            Assert.That(slot.Content, Is.EqualTo(expectedResult));
        }
    }
}
