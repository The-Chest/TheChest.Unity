using TheChest.ConsoleApp.Items;
using TheChest.Inventories.Slots;

namespace TheChest.ConsoleApp.Inventories.Stack.Lazy
{
    public class InventoryLazyStackSlot : InventoryLazyStackSlot<Item>
    {
        public InventoryLazyStackSlot(Item item,int amount , int maxStackAmount) : base(item, amount, maxStackAmount) { }
    }
}
