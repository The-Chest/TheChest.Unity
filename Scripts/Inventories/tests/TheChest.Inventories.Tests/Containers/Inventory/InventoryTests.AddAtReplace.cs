namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1, true)]
        [TestCase(-1, false)]
        [TestCase(22, true)]
        [TestCase(22, false)]
        public void AddAtReplace_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index, bool replace)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.AddAt(this.itemFactory.CreateDefault(), index, replace),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAtReplace_NullItem_ThrowsArgumentNullException(bool replace)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.AddAt(default!, 0, replace), Throws.ArgumentNullException);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAtReplace_EmptySlot_AddsItem(bool replace)
        {
            var size = this.random.Next(10,20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, randomIndex, replace);

            Assert.That(inventory[randomIndex].Content, Is.EqualTo(item));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAtReplace_EmptySlot_CallsOnAddEvent(bool replace)
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                });
            };
            inventory.AddAt(item, randomIndex, replace);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAtReplace_EmptySlot_ReturnsNull(bool replace)
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, randomIndex, replace);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddAtReplace_FullSlotReplaceTrue_ReplacesTheItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, randomIndex, true);

            Assert.Multiple(() =>
            {
                var randomSlot = inventory[randomIndex];
                Assert.That(randomSlot.Content, Is.EqualTo(item));
                Assert.That(randomSlot.Content, Is.Not.EqualTo(oldItem));
            });
        }

        [Test]
        public void AddAtReplace_FullSlotReplaceTrue_ReturnsOldItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            var result = inventory.AddAt(item, randomIndex, true);

            Assert.That(result, Is.EqualTo(oldItem));
        }

        [Test]
        public void AddAtReplace_FullSlotReplaceFalse_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if inventory is full");
            inventory.AddAt(item, randomIndex, false);
        }

        [Test]
        public void AddAtReplace_FullSlotReplaceFalse_DoesNotReplaceTheItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, randomIndex, false);

            Assert.Multiple(() =>
            {
                var randomSlot = inventory[randomIndex];
                Assert.That(randomSlot.Content, Is.Not.EqualTo(item));
                Assert.That(randomSlot.Content, Is.EqualTo(oldItem));
            });
        }

        [Test]
        public void AddAtReplace_FullSlotReplaceFalse_ReturnsSameItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, randomIndex, false);

            Assert.That(result, Is.EqualTo(item));
        }
    }
}
