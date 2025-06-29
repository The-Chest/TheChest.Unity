using TheChest.Slots.Generics.Interfaces;

namespace TheChest.Containers.Generics.Interfaces
{
    /// <summary>
    /// Interface with the basics of a container
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public interface IContainer<T>
    {
        /// <summary>
        /// Slots in the Inventory
        /// </summary>
        ISlot<T>[] Slots { get; }

        /// <summary>
        /// Size of the current Inventory
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Verify if the container is full
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Verify if the container is empty
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets an item from this.Slots
        /// </summary>
        /// <param name="index">Index of a slot<para>It needs to be smaller than this.Size</para></param>
        /// <returns>An item from this.Slots</returns>
        ISlot<T> this[int index] { get; }
    }
}
