using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Factories.Interfaces
{
    public interface IInventoryLazyStackSlotFactory<T>
    {
        IInventoryLazyStackSlot<T> EmptySlot();
        IInventoryLazyStackSlot<T> EmptySlot(int maxAmount = 10);
        IInventoryLazyStackSlot<T> WithItem(T item, int amount = 1, int maxAmount = 10);
        IInventoryLazyStackSlot<T> FullSlot(T item, int maxAmount = 10);
    }
}
