using TheChest.ConsoleApp.Items;
using TheChest.Inventories.Containers;

namespace TheChest.ConsoleApp.Inventories.Stack.Lazy
{
    public class LazyStackInventory : LazyStackInventory<Item>
    {
        public LazyStackInventory(InventoryLazyStackSlot[] slots) : base(slots) { }
    }
}
