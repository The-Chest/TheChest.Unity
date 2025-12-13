namespace TheChest.Inventories.Tests.Slots
{
    public abstract partial class IInventoryLazyStackSlotTests<T>
    {
        protected readonly IInventoryLazyStackSlotFactory<T> slotFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected readonly Random random;

        public IInventoryLazyStackSlotTests(IInventoryLazyStackSlotFactory<T> slotFactory, ISlotItemFactory<T> itemFactory)
        {
            this.slotFactory = slotFactory;
            this.itemFactory = itemFactory;

            this.random = new Random();
        }
    }
}
