using NUnit.Framework.Internal;
using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_EmptyInventory_CallsOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty inventory");
            inventory.Clear();
        }

        [Test]
        public void Clear_InventoryWithItems_ReturnsAllItems()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var result = inventory.Clear();

            Assert.That(result, Has.Length.EqualTo(size * stackSize).And.All.EqualTo(item));
        }

        [Test]
        public void Clear_InventoryWithItems_CallsOnGetEvent()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(size));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.EqualTo(stackSize));
                });
            };
            inventory.Clear();
        }

        [Test]
        public void Clear_InventoryWithItems_RemoveItemsFromAllSlots()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            inventory.Clear();

            Assert.Multiple(() =>
            {
                Assert.That(inventory.Slots,
                    Has.All.Matches<IStackSlot<T>>(
                        slot => slot.Content!.Count == 0 &&  slot.StackAmount == 0
                    )
                );
                Assert.That(inventory.IsEmpty, Is.True);
            });
        }
    }
}
