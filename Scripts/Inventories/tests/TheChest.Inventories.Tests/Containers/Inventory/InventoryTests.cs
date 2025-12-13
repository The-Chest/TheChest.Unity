namespace TheChest.Inventories.Tests.Containers
{
    public abstract partial class InventoryTests<T>
    {
        protected readonly IInventoryFactory<T> containerFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected readonly Random random;
        protected InventoryTests(IInventoryFactory<T> containerFactory, ISlotItemFactory<T> itemFactory) 
        { 
            this.containerFactory = containerFactory;
            this.itemFactory = itemFactory;

            this.random = new Random();
        }
    }
}
