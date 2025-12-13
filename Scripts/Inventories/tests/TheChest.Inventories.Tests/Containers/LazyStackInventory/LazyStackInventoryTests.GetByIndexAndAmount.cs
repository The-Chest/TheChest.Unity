namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByIndexAndAmount_ValidIndexEmptySlot_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer(20);
            var index = this.random.Next(0, 20);
            var amount = this.random.Next(1, 10);

            var result = inventory.Get(index, amount);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexEmptySlot_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer(20);
            var index = this.random.Next(0, 20);
            var amount = this.random.Next(1, 10);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty slot.");
            inventory.Get(index, amount);
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmount_ReturnsCorrectAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 10, item);
            var index = this.random.Next(0, 20);
            var amount = this.random.Next(1, 10);

            var result = inventory.Get(index, amount);

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmount_CallsOnGetEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 10, item);
            var index = this.random.Next(0, 20);
            var amount = this.random.Next(1, 10);

            inventory.OnGet += (sender, args) => {
                Assert.That(sender, Is.EqualTo(inventory));
                var firstData = args.Data.FirstOrDefault();
                Assert.Multiple(() =>
                {
                    Assert.That(firstData.Item, Is.EqualTo(item));
                    Assert.That(firstData.Index, Is.EqualTo(index));
                    Assert.That(firstData.Amount, Is.EqualTo(amount));
                });
            };
            inventory.Get(index, amount);
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmountBiggerThanSlotSize_ReturnsMaxAvailableAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAvailabeSize = 10;
            var inventory = this.containerFactory.FullContainer(20, maxAvailabeSize, item);
            var index = this.random.Next(0, 20);
            var amount = this.random.Next(maxAvailabeSize + 1, maxAvailabeSize + 10);

            var result = inventory.Get(index, amount);

            Assert.That(result, Has.Length.EqualTo(maxAvailabeSize));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmountBiggerThanSlotSize_CallsOnGetEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAvailabeSize = 10;
            var inventory = this.containerFactory.FullContainer(20, maxAvailabeSize, item);
            var index = this.random.Next(0, 20);
            var amount = this.random.Next(maxAvailabeSize + 1, maxAvailabeSize + 10);

            inventory.OnGet += (sender, args) => {
                Assert.That(sender, Is.EqualTo(inventory));
                var firstData = args.Data.FirstOrDefault();
                Assert.Multiple(() =>
                {
                    Assert.That(firstData.Item, Is.EqualTo(item));
                    Assert.That(firstData.Index, Is.EqualTo(index));
                    Assert.That(firstData.Amount, Is.EqualTo(maxAvailabeSize));
                });
            };
            inventory.Get(index, amount);
        }

        [TestCase(-1, 1)]
        [TestCase(2000, 1)]
        public void Get_ByIndexAndAmount_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.Get(index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("index")
            );
        }

        [TestCase(1, 0)]
        [TestCase(0, -1)]
        public void Get_ByIndexAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.Get(index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("amount")
            );
        }
    }
}
