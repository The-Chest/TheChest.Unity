using TheChest.Inventories.Slots.Interfaces;
using TheChest.Core.Slots;

namespace TheChest.Inventories.Tests.Slots.Factories
{
    public class InventorySlotFactory<Slot, Item> : IInventorySlotFactory<Item> 
        where Slot : Slot<Item>
    {
        public virtual IInventorySlot<Item> EmptySlot()
        {
            var slot = Activator.CreateInstance(typeof(Slot), default(Item));
            return (IInventorySlot<Item>)slot!;
        }

        public virtual IInventorySlot<Item> FullSlot(Item item)
        {
            var constructor = typeof(Slot).GetConstructor(new Type[1] { typeof(Item?) });
            var slot = constructor!.Invoke(new object[1] { item });
            return (IInventorySlot<Item>)slot;
        }
    }
}
