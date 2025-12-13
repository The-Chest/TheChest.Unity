using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Add_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.Add(default!));
        }

        [Test]
        public void Add_SuccessfullyAdded_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Add(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Add_EmptyInventory_AddsToFirstAvilableSlot()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);
            
            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Has.All.EqualTo(item));
                Assert.That(inventory[0].IsEmpty, Is.False);
                Assert.That(inventory[0].StackAmount, Is.EqualTo(1));
            });
        }

        [Test]
        public void Add_EmptyInventory_CallsOnAddEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                    Assert.That(firstEvent.Amount, Is.EqualTo(1));
                });
            };  
            inventory.Add(item);
        }

        [Test]
        public void Add_InventoryWithAvailableSlots_AddsToFirstAvilableSlot()
        {
            var stackAmount = this.random.Next(1, 5);
            var inventorySize = this.random.Next(2, 20);
            var items = this.itemFactory.CreateManyRandom(inventorySize - 1);
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackAmount, items);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.Slots,
                    Has.One.Matches<IStackSlot<T>>(
                        slot =>
                            slot.Content!.All(content => content?.Equals(item) ?? false) && 
                            slot.StackAmount == 1
                    )
                );
            });
        }

        [Test]
        public void Add_FailedToAdd_ReturnsFalse()
        {
            var items = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(10,5, items);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Add(item);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Add_FullInventory_DoesNotAddsToInventory()
        {
            var items = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(10, 5, items);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.Slots, 
                    Has.All.Matches<IStackSlot<T>>(
                        slot => slot.IsFull && slot.Content!.All(item => item.Equals(items))
                    )
                );
            });
        }

        [Test]
        public void Add_FullInventory_DoesNotCallOnAddEvent()
        {
            var items = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(10, 5, items);

            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when inventory is full.");
            inventory.Add(item);
        }
    }
}
