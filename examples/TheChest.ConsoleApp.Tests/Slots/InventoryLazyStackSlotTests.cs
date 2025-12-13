using TheChest.ConsoleApp.Inventories.Stack.Lazy;
using TheChest.Core.Tests.Slots.Factories.Interfaces;
using TheChest.Inventories.Tests.Slots;

namespace TheChest.ConsoleApp.Tests.Slots
{
    [TestFixtureSource(nameof(SlotFixtureArgs))]
    public class InventoryLazyStackSlotTests : IInventoryLazyStackSlotTests<Item>
    {
        static readonly object[] SlotFixtureArgs = {
            new object[] {
                new InventoryLazyStackSlotFactory<InventoryLazyStackSlot, Item>(),
                new SlotItemFactory<Item>(),
            }
        };
        public InventoryLazyStackSlotTests(IInventoryLazyStackSlotFactory<Item> slotFactory, ISlotItemFactory<Item> itemFactory) : 
            base(slotFactory, itemFactory)
        {
        }
    }
}
