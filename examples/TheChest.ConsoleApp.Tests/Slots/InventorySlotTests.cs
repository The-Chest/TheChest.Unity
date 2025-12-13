using TheChest.Inventories.Tests.Slots;
using TheChest.Core.Tests.Slots.Factories.Interfaces;

namespace TheChest.ConsoleApp.Tests.Slots
{
    [TestFixtureSource(nameof(SlotFixtureArgs))]
    public class InventorySlotTests : IInventorySlotTests<Item>
    {
        static readonly object[] SlotFixtureArgs = {
            new object[] {
                new InventorySlotFactory<InventorySlot, Item>(),
                new SlotItemFactory<Item>(),
            }
        };
        public InventorySlotTests(IInventorySlotFactory<Item> slotFactory, ISlotItemFactory<Item> itemFactory) : base(slotFactory, itemFactory)
        {
        }
    }
}
