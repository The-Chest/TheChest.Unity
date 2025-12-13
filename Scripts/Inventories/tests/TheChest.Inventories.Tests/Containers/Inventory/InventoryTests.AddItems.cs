namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void AddItems_NoItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(0);

            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_NoItems_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(0);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if no items are added");
            inventory.Add(items);
        }

        [Test]
        public void AddItems_ArrayWithOnlyNullItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = new T[10];
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_ArrayWithOnlyNullItems_DoesNotAddToAnySlot()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = new T[10];
            inventory.Add(items);

            Assert.That(inventory.Slots.All(slot => slot.IsEmpty), Is.True);
        }

        [Test]
        public void AddItems_ArrayWithOnlyNullItems_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if no items are added");
            inventory.Add(new T[10]);
        }

        [Test]
        public void AddItems_ArrayContainingNullItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = this.itemFactory
                .CreateManyRandom(10)
                .Append(default!)
                .Reverse()
                .ToArray();
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_ArrayContainingNullItems_DoesNotAddNullToAnySlot()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomItemSize = this.random.Next(3, size);
            var items = this.itemFactory
                .CreateManyRandom(randomItemSize)
                .Append(default!)
                .Reverse()
                .ToArray();
            inventory.Add(items);
 
            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Is.EqualTo(items[1]));
                Assert.That(inventory[randomItemSize - 1].Content, Is.EqualTo(items[randomItemSize - 2]));
            });
        }

        [Test]
        public void AddItems_ArrayContainingNullItems_DoesNotCallOnAddEventWithTheNullItem()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.None.Null);
                });
            };

            var randomItemSize = this.random.Next(2, size);
            var items = this.itemFactory
                .CreateManyRandom(randomItemSize)
                .Append(default!)
                .Reverse()
                .ToArray();
            inventory.Add(items);
        }

        [Test]
        public void AddItems_ArrayContainingNullItems_CallsOnAddEventWithAddedItems()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomItemSize = this.random.Next(2, size);
            var items = this.itemFactory
                .CreateManyRandom(randomItemSize)
                .Append(default!)
                .Reverse()
                .ToArray();

            var countIndex = 1;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(items[countIndex++]));
                });
            };
            inventory.Add(items);
        }

        [Test]
        public void AddItems_EmptySlots_AddsAllItems()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            inventory.Add(items);

            Assert.That(inventory.Slots.Skip(0).Take(randomSize).Select(x => x.Content), Is.EqualTo(items));
        }

        [Test]
        public void AddItems_EmptySlots_CallsOnAddForAllItems()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Is.EqualTo(items));
                    Assert.That(args.Data, Has.Count.EqualTo(randomSize));
                });
            };
            inventory.Add(items);
        }

        [Test]
        public void AddItems_SuccessAdding_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_NotAvailabeSlotsForAllItems_CallsOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var emptySlotsSize = this.random.Next(1, size);
            var itemSize = size - emptySlotsSize;
            var items = this.itemFactory.CreateMany(itemSize);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            var addSize = emptySlotsSize + 1;
            var manyAdded = this.itemFactory.CreateManyRandom(addSize);
            var countIndex = 1;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(manyAdded[countIndex++]));
                });
            };
            inventory.Add(manyAdded);
        }

        [Test]
        public void AddItems_NotAvailabeSlotsForAllItems_AddsSomeItems()
        {
            var size = this.random.Next(10, 20);
            var emptySlotsSize = this.random.Next(1, size);
            var itemSize = size - emptySlotsSize;
            var items = this.itemFactory.CreateMany(itemSize);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            var addSize = emptySlotsSize + 1;
            var manyAdded = this.itemFactory.CreateManyRandom(addSize);
            inventory.Add(manyAdded);

            Assert.That(inventory.GetCount(manyAdded[0]), Is.EqualTo(emptySlotsSize));
        }

        [Test]
        public void AddItems_NotAvailabeSlotsForAllItems_ReturnsNotAddedItems()
        {
            var size = this.random.Next(10, 20);
            var emptySlotsSize = this.random.Next(1, size);
            var itemSize = size - emptySlotsSize;
            var items = this.itemFactory.CreateMany(itemSize);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            var manyAdded = this.itemFactory.CreateManyRandom(emptySlotsSize + 1);
            var result = inventory.Add(manyAdded);

            Assert.That(result, Has.Length.EqualTo(1));
        }

        [Test]
        public void AddItems_FullInventory_DoNotAddItems()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateManyRandom(size);
            inventory.Add(items);

            Assert.That(inventory.GetCount(items[0]), Is.EqualTo(0));
        }

        [Test]
        public void AddItems_FullInventory_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateManyRandom(size);
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if no items are added");
            inventory.Add(items);
        }

        [Test]
        public void AddItems_FullInventory_ReturnsAllNotAddedItems()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateMany(size);
            var result = inventory.Add(items);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
