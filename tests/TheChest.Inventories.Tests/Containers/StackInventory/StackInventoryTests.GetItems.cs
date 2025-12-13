namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetItems_WhenAmountIsZeroOrSmaller_ThrowsArgumentOutOfRangeException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.Get(item, 0), Throws.TypeOf(typeof(ArgumentOutOfRangeException)));
        }

        [Test]
        public void GetItems_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.Get(default(T)!, 1), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItems_ItemNotFound_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer();

            var items = inventory.Get(item, 1);

            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetItems_ItemNotFound_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            var item = this.itemFactory.CreateDefault();
            inventory.Get(item, 1);
        }

        [Test]
        public void GetItems_ItemsFound_ReturnsItems()
        {
            var amount = 10;
            var stackSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateManyRandom(amount);
            var inventoryItems = this.itemFactory.CreateManyRandom(10).ToList();
            inventoryItems.AddRange(items);

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());

            var expectedItem = items[0];
            var result = inventory.Get(expectedItem, amount);

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(expectedItem));
        }

        [Test]
        public void GetItems_ItemsFound_RemovesItems()
        {
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(10)
                .Append(item)
                .ToList();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());

            inventory.Get(item, stackSize);

            Assert.That(inventory.Slots.Any(x => x.Content?.Contains(item) ?? false), Is.False);
        }

        [Test]
        public void GetItems_ItemsFound_CallsOnGetEvent()
        {
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(10)
                .Append(item)
                .ToList();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault(); 
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize).And.All.EqualTo(item));
                });
            };
            inventory.Get(item, stackSize);
        }

        [Test]
        public void GetItems_AmoutBiggerThanItemsInInventory_ReturnsAllItemsFound()
        {
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(10)
                .Append(item)
                .ToList();

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());
            var result = inventory.Get(item, 100);

            Assert.That(result, Has.Length.EqualTo(stackSize));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void GetItems_AmoutBiggerThanItemsInInventory_CallsOnGetEvent()
        {
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(10)
                .Append(item)
                .ToList();

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(args.Data.First().Items, Has.All.EqualTo(item));
                });
            };
            inventory.Get(item, 100);
        }
    }
}
