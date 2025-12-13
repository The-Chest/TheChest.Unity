namespace TheChest.Inventories.Tests.Slots
{
    public abstract partial class IInventorySlotTests<T>
    {
        protected readonly IInventorySlotFactory<T> slotFactory;
        protected readonly ISlotItemFactory<T> itemFactory;
        
        protected readonly Random random;

        public IInventorySlotTests(IInventorySlotFactory<T> slotFactory, ISlotItemFactory<T> itemFactory) 
        {
            this.slotFactory = slotFactory;
            this.itemFactory = itemFactory;
        
            this.random = new Random();
        }
    }
}
