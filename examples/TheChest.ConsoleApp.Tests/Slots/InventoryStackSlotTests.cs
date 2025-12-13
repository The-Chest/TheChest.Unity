using TheChest.Core.Tests.Slots.Factories.Interfaces;
using TheChest.Inventories.Tests.Slots;

namespace TheChest.ConsoleApp.Tests.Slots
{
    [TestFixtureSource(nameof(SlotFixtureArgs))]
    public class InventoryStackSlotTests : IInventoryStackSlotTests<Item>
    {
        static readonly object[] SlotFixtureArgs = {
            new object[] {
                new InventoryStackSlotFactory<InventoryStackSlot, Item>(),
                new SlotItemFactory<Item>(),
            }
        };
        public InventoryStackSlotTests(IInventoryStackSlotFactory<Item> slotFactory, ISlotItemFactory<Item> itemFactory) : base(slotFactory, itemFactory)
        {
        }
    }
}
