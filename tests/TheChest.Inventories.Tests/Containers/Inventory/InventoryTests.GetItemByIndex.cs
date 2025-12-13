using TheChest.Inventories.Containers.Events;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(30)]
        public void GetItemByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Get(index), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void GetItemByIndex_ValidIndexEmptySlot_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer();
            var index = this.random.Next(0, size);
            inventory.Get(index);
            inventory.OnGet += (sender, args) => Assert.Fail("Get(int index) should not be called if no item is found");
        }

        [Test]
        public void GetItemByIndex_ValidIndexFullSlot_ReturnsItem()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Get(randomIndex);
           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(item));
                Assert.That(inventory[randomIndex].IsEmpty, Is.True);
            });
        }

        [Test]
        public void GetItemByIndex_ValidIndexEmptySlot_ReturnsNull()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetItemByIndex_ExistingItemOnSlot_CallsOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(
                    args.Data, 
                    Has.Count.EqualTo(1)
                        .And.All.EqualTo(new InventoryGetItemEventData<T>(item, randomIndex))
                );
            };
            inventory.Get(randomIndex);
        }
    }
}
