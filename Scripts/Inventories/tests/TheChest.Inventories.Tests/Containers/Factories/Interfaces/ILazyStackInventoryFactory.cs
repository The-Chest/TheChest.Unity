using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Factories.Interfaces
{
    public interface ILazyStackInventoryFactory<T>
    {
        ILazyStackInventory<T> EmptyContainer(int size = 20, int stackSize = 5);
        ILazyStackInventory<T> FullContainer(int size, int stackSize, T item);
        ILazyStackInventory<T> ShuffledItemsContainer(int size, int stackSize, params T[] items);
    }
}
