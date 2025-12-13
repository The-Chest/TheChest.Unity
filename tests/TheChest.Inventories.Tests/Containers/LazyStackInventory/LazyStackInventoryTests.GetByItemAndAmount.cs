using System.Drawing;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByItemAndAmount_Nulltem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.Get(item: default!, amount: 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Get_ByItemAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Get(item, amount));
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItemsAndAmountBiggerThanInventorySize_ReturnsMaxAvailableAmount()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var amount = size * stackSize * 2;
            var expectedAmount = size * stackSize;
            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(expectedAmount).And.All.EqualTo(item));
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItemsAndAmountBiggerThanInventorySize_CallsOnGetEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var amount = size * stackSize * 2;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(size));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Index), Has.All.InRange(0, size));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.EqualTo(stackSize));
                });
            };
            inventory.Get(item, amount);
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItems_ReturnsCorrectAmountFromMultipleSlots()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);
            var amount = this.random.Next(2, 20);

            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(amount).And.All.EqualTo(item));
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItems_CallsOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);
            var multiplier = this.random.Next(2, size);
            var amount = stackSize * multiplier;
            
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(multiplier));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Index), Has.All.InRange(0, size));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.EqualTo(stackSize));
                });
            };
            inventory.Get(item, amount);
        }

        [Test]
        public void Get_ByItemAndAmount_NotFoundItem_ReturnsEmptyArray()
        {
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, 1, randomItem);

            var amount = this.random.Next(2, 20);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.Get(item, amount);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Get_ByItemAndAmount_NotFoundItem_DoesNotCallOnGetEvent()
        {
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, 1, randomItem);

            var amount = this.random.Next(2, 20);
            var item = this.itemFactory.CreateDefault();

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for non-existing item.");
            inventory.Get(item, amount);
        }
    }
}
