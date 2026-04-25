using System;
using System.Collections.Generic;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Slots.Extensions
{
    internal static class IInventoryLazyStackSlotExtensions
    {
        /// <summary>
        /// Determines the indexes of slots where the specified item can be added, prioritizing slots that already contain the item.
        /// </summary>
        /// <remarks>
        /// Slots that already contain the item are prioritized in the returned order. 
        /// This method does not modify the inventory; it only determines the order in which slots should be considered for adding the specified amount of the item.
        /// </remarks>
        /// <param name="slots">An array of inventory slots to consider for adding the item.</param>
        /// <param name="item">The item to add to the inventory slots. Cannot be null.</param>
        /// <param name="amount">The total number of items to add. Must be greater than or equal to 0.</param>
        /// <returns>
        /// <para>
        /// An enumerable collection of slot indexes, ordered to prioritize slots that already contain the item, followed by other eligible slots. 
        /// </para>
        /// <para>
        /// The collection may be empty if no suitable slots are found.
        /// </para>
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the item parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the amount parameter is less than 0.</exception>
        internal static IEnumerable<int> GetAddOrderIndexes<T>(this IInventoryLazyStackSlot<T>[] slots, T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than or equal to 0.");

            var main = new List<int>(slots.Length);
            var fallback = new List<int>(slots.Length);

            var notAddedAmount = amount;
            for (var index = 0; index < slots.Length; index++)
            {
                var slot = slots[index];
                var toAddAmount = notAddedAmount > slot.AvailableAmount ? slot.AvailableAmount : notAddedAmount;
                if (!slot.CanAdd(item, toAddAmount))
                    continue;

                if (slot.Contains(item))
                {
                    if (main.Count <= amount)
                        main.Add(index);

                    continue;
                }

                if (fallback.Count <= amount)
                    fallback.Add(index);
            }

            main.AddRange(fallback);
            return main;
        }
    }
}
