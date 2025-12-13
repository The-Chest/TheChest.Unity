using TheChest.ConsoleApp.Inventories.Stack.Lazy;
using TheChest.Core.Tests.Slots.Factories.Interfaces;
using TheChest.Inventories.Tests.Containers.LazyStackInventory;

namespace TheChest.ConsoleApp.Tests.Containers
{
    [TestFixtureSource(nameof(FixtureArgs))]
    public class LazyStackInventoryTests : LazyStackInventoryTests<Item>
    {
        static readonly object[] FixtureArgs = new object[]{
            new object[] {
                new LazyStackInventoryFactory<LazyStackInventory, Item>(
                    new InventoryLazyStackSlotFactory<InventoryLazyStackSlot, Item>()
                ),
                new SlotItemFactory<Item>(),
            },
        };

        public LazyStackInventoryTests(ILazyStackInventoryFactory<Item> containerFactory, ISlotItemFactory<Item> itemFactory) : base(containerFactory,  itemFactory)
        {
        }
    }
}
