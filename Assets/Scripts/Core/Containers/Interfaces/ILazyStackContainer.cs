using TheChest.Core.Slots.Interfaces;

namespace TheChest.Core.Containers.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILazyStackContainer<in T>
    {
        /// <summary>
        /// Size of the current Container
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
        /// Gets an item from <see cref="ILazyStackSlot{T}"/>
        /// </summary>
        /// <param name="index">Index of a slot<para>It needs to be smaller than <see cref="ILazyStackContainer{T}.Size"/></para></param>
        /// <returns>An item from <see cref="ILazyStackSlot{T}"/></returns>
        ILazyStackSlot<T> this[int index] { get; }
    }
}
