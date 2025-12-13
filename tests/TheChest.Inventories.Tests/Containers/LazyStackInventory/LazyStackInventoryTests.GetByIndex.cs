using System;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByIndex_ValidIndex_ReturnsItem()
        {
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, expectedItem);

            var result = inventory.Get(0);

            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        public void Get_ByIndex_ValidIndex_CallsOnGetEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size);
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Item, Is.EqualTo(expectedItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                    Assert.That(firstEvent.Amount, Is.EqualTo(1));
                });
            };
            inventory.Get(index);
        }

        [Test]
        public void Get_ByIndex_EmptySlot_ReturnsNull()
        {
            var inventory = this.containerFactory.EmptyContainer(20);

            var result = inventory.Get(0);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_ByIndex_EmptySlot_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer(20);
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for an empty slot.");
            inventory.Get(0);
        }

        [TestCase(-1)]
        [TestCase(1000)]
        public void Get_ByIndex_ShouldThrowArgumentOutOfRangeException_WhenIndexIsInvalid(int index)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Get(index));
        }
    }
}
