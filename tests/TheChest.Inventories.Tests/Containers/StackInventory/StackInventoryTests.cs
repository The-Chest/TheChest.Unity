namespace TheChest.Inventories.Tests.Containers
{
    public abstract partial class StackInventoryTests<T> 
    {
        protected readonly IStackInventoryFactory<T> containerFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected readonly Random random;

        public StackInventoryTests(IStackInventoryFactory<T> containerFactory, ISlotItemFactory<T> itemFactory)
        { 
            this.containerFactory = containerFactory;
            this.itemFactory = itemFactory;
        
            this.random = new Random();
        }
    }
}
