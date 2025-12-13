using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void AddItems_AddingEmptyArray_ThrowsArgumentException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var items = Array.Empty<T>();
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_EmptyInventory_ReturnsEmptyArray()
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer();

            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_EmptyInventory_AddsToFirstSlot()
        {
            var items = this.itemFactory.CreateMany(10);
            var inventory = this.containerFactory.EmptyContainer();

            inventory.Add(items);

            Assert.That(inventory[0].Content, Is.EqualTo(items));
        }

        [Test]
        public void AddItems_EmptyInventory_CallsOnAddEvent()
        {
            var items = this.itemFactory.CreateMany(10);
            var inventory = this.containerFactory.EmptyContainer();

            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(items.Length).And.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
            };
            inventory.Add(items);
        }

        [Test]
        public void AddItems_SlotWithSameItem_AddsToSlotWithItemFirst()
        {
            var item = this.itemFactory.CreateDefault();
            var maxSize = this.random.Next(2, 10);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, maxSize, item);
            inventory.Get(item, maxSize - 1);

            var amount = maxSize;
            var items = this.itemFactory.CreateMany(amount);
            inventory.Add(items);

            Assert.That(inventory.Slots, Has.One.Matches<IStackSlot<T>>(x => x.StackAmount == maxSize && x.Content!.Contains(item)));
        }

        [Test]
        public void AddItems_SlotWithSameItem_CallsOnAddEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var maxSize = this.random.Next(2, 10);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, maxSize, item);
            inventory.Get(item, maxSize - 1);

            var amount = maxSize;
            var items = this.itemFactory.CreateMany(amount);

            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(maxSize - 1).And.EqualTo(items[..^1]));
                });
                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(items[^1]));
                });
            };
            inventory.Add(items);
        }

        [Test]
        public void AddItems_FullSlotWithSameItem_AddsToSlotWithItemFirst()
        {
            var item = this.itemFactory.CreateDefault();
            var maxSize = this.random.Next(2, 10);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, maxSize, item);

            var amount = this.random.Next(1, maxSize - 1);
            var items = this.itemFactory.CreateMany(maxSize + amount);
            inventory.Add(items);

            Assert.That(
                inventory.Slots, 
                Has.Exactly(2).Matches<IStackSlot<T>>(x => x.StackAmount == maxSize && x.Content!.Contains(item))
            );
            Assert.That(
                inventory.Slots,
                Has.Exactly(1).Matches<IStackSlot<T>>(x => x.StackAmount == amount && x.Content!.Contains(item))
            );
        }

        [Test]
        public void AddItems_SlotWithSameItem_AddsToFirstAvailableSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var maxSize = this.random.Next(2, 10);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, maxSize, item);
            inventory.Get(item, maxSize - 1);

            var amount = maxSize;
            var items = this.itemFactory.CreateMany(amount);
            inventory.Add(items);

            Assert.That(inventory.Slots, Has.One.Matches<IStackSlot<T>>(x => x.StackAmount == 1 && x.Content!.Contains(item)));
        }

        [Test]
        public void AddItems_EmptyInventory_BiggerAmountThanSlotSize_AddsToAvailableSlots()
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer();

            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(2));

                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(10).And.EqualTo(items.Take(10)));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });

                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Has.Length.EqualTo(10).And.EqualTo(items.Skip(10)));
                    Assert.That(secondEvent.Index, Is.EqualTo(1));
                });
            };
            inventory.Add(items);
        }

        [Test]
        public void AddItems_EmptyInventory_BiggerAmountThanSlotSize_CallsOnAddEvent()
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer();

            inventory.Add(items);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Is.EqualTo(items.Take(10)));
                Assert.That(inventory[1].Content, Is.EqualTo(items.Skip(10)));
            });
        }

        [Test]
        public void AddItems_FullInventory_DoNotAddsItems()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var items = this.itemFactory.CreateMany(20);
            var expectedItems = items.ToArray();
            var inventory = this.containerFactory.FullContainer(20, 2, slotItem);

            inventory.Add(items);

            Assert.That(items, Is.EqualTo(expectedItems));
        }

        [Test]
        public void AddItems_FullInventory_DoNotCallOnAddEvent()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.FullContainer(20, 2, slotItem);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");
            inventory.Add(items);
        }

        [Test]
        public void AddItems_FullInventory_ReturnsNotAddedItems()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.FullContainer(20, 2, slotItem);

            var result = inventory.Add(items);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
