using TheChest.ConsoleApp.Items;
using TheChest.Inventories.Containers;

namespace TheChest.ConsoleApp.Inventories.Stack
{
    public class StackInventory : StackInventory<Item>
    {
        public StackInventory(InventoryStackSlot[] slots) : base(slots) { }
    }
}
