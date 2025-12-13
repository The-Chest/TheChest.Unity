using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Factories.Interfaces
{
    public interface IStackInventoryFactory<T>
    {
        IStackInventory<T> EmptyContainer(int size = 20, int stackSize = 10);
        IStackInventory<T> FullContainer(int size, int stackSize, T item = default!);
        IStackInventory<T> ShuffledItemsContainer(int size, int stackSize, params T[] items);
    }
}
