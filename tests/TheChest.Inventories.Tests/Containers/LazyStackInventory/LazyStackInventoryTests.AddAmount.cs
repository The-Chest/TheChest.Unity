using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Add_WithAmount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.Add(item: default!, amount: 1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Add_WithAmount_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Add(item, amount));
        }

        [Test]
        public void Add_WithAmount_EmptyInventory_AddsToFirstAvilableSlot()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            inventory.Add(item, amount);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Has.All.EqualTo(item));
                Assert.That(inventory[0].IsEmpty, Is.False);
                Assert.That(inventory[0].StackAmount, Is.EqualTo(amount));
            });
        }

        [Test]
        public void Add_WithAmount_EmptyInventory_CallsOnAddEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            inventory.OnAdd += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                    Assert.That(firstEvent.Amount, Is.EqualTo(amount));
                });
            };
            inventory.Add(item, amount);
        }

        [Test]
        public void Add_WithAmount_SuccessfullyAddedItems_ReturnsZero()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);

            var result = inventory.Add(item, amount);

            Assert.That(result, Is.Zero);
        }

        [Test]
        public void Add_WithAmount_FullInventory_ReturnsRemainingAmount()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            var result = inventory.Add(item, amount);

            Assert.That(result, Is.EqualTo(amount));
        }

        [Test]
        public void Add_WithAmount_FullInventory_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called when inventory is full.");
            inventory.Add(item, amount);
        }

        [Test]
        public void Add_WithAmount_FullInventory_DoesNotAddToInventory()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            inventory.Add(item, amount);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.Slots,
                    Has.All.Matches<IStackSlot<T>>(
                        slot => slot.IsFull && slot.Content!.All(item => item.Equals(randomItem))
                    )
                );
            });
        }

        [Test]
        public void Add_WithAmount_NotAllItemsBeAdded_ReturnsRemainingAmount()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(2, 10);
            var randomItem = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);
            var result = inventory.Add(item, amount);

            Assert.That(result, Is.EqualTo(amount - stackSize));
        }

        [Test]
        public void Add_WithAmount_NotAllItemsBeAdded_CallsOnAddEvent()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(2, 10);
            var randomItem = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.InRange(0, size));
                    Assert.That(firstEvent.Amount, Is.EqualTo(stackSize));
                });
            };
            inventory.Add(item, amount);
        }

        [Test]
        public void Add_WithAmount_NotAllItemsBeAdded_AddsToFirstAvailableSlot()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);
            inventory.Add(item, amount);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.Slots,
                    Has.One.Matches<IStackSlot<T>>(
                        slot =>
                            slot.Content!.All(content => content?.Equals(item) ?? false) &&
                            slot.StackAmount == amount - (amount - stackSize)
                    )
                );
            });
        }
    }
}
