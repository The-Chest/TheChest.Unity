namespace TheChest.Inventories.Extensions
{
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Checks if the array contains any null values.
        /// </summary>
        /// <param name="array">An array of any type.</param>
        /// <returns>Returns true if the array contains at least one null value, otherwise false.</returns>
        internal static bool ContainsNull<T>(this T[] array) 
        {
            if (array.Length == 0)
                return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] is null)
                    return true;
            }

            return false;
        }
    }
}
