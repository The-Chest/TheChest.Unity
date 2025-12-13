namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public abstract partial class LazyStackInventoryTests<T>
    {
        protected readonly ILazyStackInventoryFactory<T> containerFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected readonly Random random;
        protected LazyStackInventoryTests(ILazyStackInventoryFactory<T> containerFactory, ISlotItemFactory<T> itemFactory)
        {
            this.containerFactory = containerFactory;
            this.itemFactory = itemFactory;

            this.random = new Random();
        }
    }
}
