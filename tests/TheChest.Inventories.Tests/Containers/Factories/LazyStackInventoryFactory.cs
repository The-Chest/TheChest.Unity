using TheChest.Inventories.Containers;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Factories
{
    public class LazyStackInventoryFactory<LazyStackInventory, Item> : ILazyStackInventoryFactory<Item>
        where LazyStackInventory : LazyStackInventory<Item>
    {
        private readonly IInventoryLazyStackSlotFactory<Item> slotFactory;
        public LazyStackInventoryFactory(IInventoryLazyStackSlotFactory<Item> slotFactory)
        {
            this.slotFactory = slotFactory;
        }

        private static Type GetInventoryType()
        {
            var inventoryType = typeof(LazyStackInventory);
            if (!typeof(ILazyStackInventory<Item>).IsAssignableFrom(inventoryType))
            {
                throw new ArgumentException($"Type '{inventoryType.FullName}' does not implement ILazyStackInventory<{typeof(Item).Name}>.");
            }

            return inventoryType;
        }

        private static Type GetSlotTypeFromConstructor()
        {
            var inventoryType = GetInventoryType();

            var constructor = inventoryType.GetConstructors()
                    .FirstOrDefault(ctor =>
                    {
                        var parameters = ctor.GetParameters();
                        return
                            parameters.Length == 1 &&
                            parameters[0].ParameterType.IsArray &&
                            typeof(IInventoryLazyStackSlot<Item>).IsAssignableFrom(parameters[0].ParameterType.GetElementType());
                    })
                    ?? throw new ArgumentException($"Inventory type '{inventoryType.FullName}' does not have a suitable constructor.");

            var slotParameter = constructor.GetParameters().FirstOrDefault(x => x.Name == "slots")
                ?? throw new ArgumentException($"Inventory type '{inventoryType.FullName}' does not have a constructor with IInventoryLazyStackSlot<{typeof(Item).Name}>[].");

            var slotType = slotParameter.ParameterType.GetElementType();
            if (!typeof(IInventoryLazyStackSlot<Item>).IsAssignableFrom(slotType))
            {
                throw new ArgumentException($"Type '{slotType!.FullName}' does not implement ILazyStackInventory<{typeof(Item).Name}>.");
            }

            return slotType;
        }

        public virtual ILazyStackInventory<Item> EmptyContainer(int size = 20, int stackSize = 5)
        {
            var containerType = GetInventoryType();
            var slotType = GetSlotTypeFromConstructor();

            Array slots = Array.CreateInstance(slotType, size);
            for (int index = 0; index < size; index++)
            {
                slots.SetValue(slotFactory.EmptySlot(stackSize), index);
            }

            var container = Activator.CreateInstance(
                type: containerType,
                args: new object[1] { slots }
            );
            return (ILazyStackInventory<Item>)container!;
        }

        public virtual ILazyStackInventory<Item> FullContainer(int size, int stackSize, Item item)
        {
            var containerType = GetInventoryType();
            var slotType = GetSlotTypeFromConstructor();

            Array slots = Array.CreateInstance(slotType, size);
            for (int i = 0; i < size; i++)
            {
                var slot = slotFactory.WithItem(item, stackSize, stackSize);
                slots.SetValue(slot, i);
            }

            var container = Activator.CreateInstance(containerType, slots);

            return (ILazyStackInventory<Item>)container!;
        }

        public virtual ILazyStackInventory<Item> ShuffledItemsContainer(int size, int stackSize, params Item[] items)
        {
            if(items.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(items), "Items cannot be empty.");
            var containerType = GetInventoryType();

            var slotType = GetSlotTypeFromConstructor();

            Array slots = Array.CreateInstance(slotType, size);
            for (int index = 0; index < size; index++)
            {
                if(index >= items.Length)
                {
                    slots.SetValue(slotFactory.EmptySlot(stackSize), index);
                }
                else
                {
                    slots.SetValue(
                        slotFactory.WithItem(
                            item: items[index], 
                            amount: Random.Shared.Next(1, stackSize), 
                            maxAmount: stackSize
                        ),
                        index
                     );
                }
            }

            var container = Activator.CreateInstance(containerType, slots);

            return (ILazyStackInventory<Item>)container!;
        }
    }
}
