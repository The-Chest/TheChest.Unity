using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Factories.Interfaces
{
    public interface IInventoryStackSlotFactory<T>
    {
        IInventoryStackSlot<T> EmptySlot();
        IInventoryStackSlot<T> EmptySlot(int maxAmount = 10);
        IInventoryStackSlot<T> WithItem(T item, int amount = 1, int maxAmount = 10);
        IInventoryStackSlot<T> WithItems(T[] items, int maxAmount = 10);
        /// <summary>
        /// Creates an <see cref="IInventoryStackSlot{T}"/> with the max supported amount of items inside it
        /// </summary>
        /// <param name="item">The item that will be inside the created based on param <paramref name="maxAmount"/> times inside it <see cref="IStackSlot{T}"/></param>
        /// <param name="maxAmount">Max amount of <paramref name="item"/></param>
        /// <returns>A full <see cref="IInventoryStackSlot{T}"/></returns>
        IInventoryStackSlot<T> FullSlot(T item, int maxAmount = 10);
        IInventoryStackSlot<T> FullSlot(T[] items);
    }
}
