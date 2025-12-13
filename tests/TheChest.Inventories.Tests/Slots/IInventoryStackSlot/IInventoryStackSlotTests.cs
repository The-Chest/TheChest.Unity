namespace TheChest.Inventories.Tests.Slots
{
    public abstract partial class IInventoryStackSlotTests<T>
    {
        protected readonly IInventoryStackSlotFactory<T> slotFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected readonly Random random; 
        
        public IInventoryStackSlotTests(IInventoryStackSlotFactory<T> slotFactory, ISlotItemFactory<T> itemFactory)
        {
            this.slotFactory = slotFactory;
            this.itemFactory = itemFactory;

            this.random = new Random();
        }
    }
}
