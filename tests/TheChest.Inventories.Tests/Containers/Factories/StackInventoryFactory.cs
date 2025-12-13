using TheChest.Inventories.Containers;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;
using TheChest.Inventories.Tests.Extensions;

namespace TheChest.Inventories.Tests.Containers.Factories
{
    public class StackInventoryFactory<T, Y> : IStackInventoryFactory<Y>
        where T : StackInventory<Y>
    {
        protected readonly IInventoryStackSlotFactory<Y> slotFactory;

        public StackInventoryFactory(IInventoryStackSlotFactory<Y> slotFactory) 
        {
            this.slotFactory = slotFactory;
        }

        private static Type GetInventoryType()
        {
            var inventoryType = typeof(T);
            if (!typeof(IStackInventory<Y>).IsAssignableFrom(inventoryType))
            {
                throw new ArgumentException($"Type '{inventoryType.FullName}' does not implement IInventory<{typeof(Y).Name}>.");
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
                            typeof(IInventoryStackSlot<Y>).IsAssignableFrom(parameters[0].ParameterType.GetElementType());
                    })
                    ?? throw new ArgumentException($"Inventory type '{inventoryType.FullName}' does not have a suitable constructor.");

            var slotParameter = constructor.GetParameters().FirstOrDefault(x => x.Name == "slots")
                ?? throw new ArgumentException($"Inventory type '{inventoryType.FullName}' does not have a constructor with IInventorySlot<{typeof(Y).Name}>[].");

            var slotType = slotParameter.ParameterType.GetElementType();
            if (!typeof(IInventoryStackSlot<Y>).IsAssignableFrom(slotType))
            {
                throw new ArgumentException($"Type '{slotType!.FullName}' does not implement IInventoryStackSlot<{typeof(Y).Name}>.");
            }

            return slotType;
        }

        public virtual IStackInventory<Y> EmptyContainer(int size = 20, int stackSize = 10)
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
            return (IStackInventory<Y>)container!;
        }

        public virtual IStackInventory<Y> FullContainer(int size, int stackSize, Y item)
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

            return (IStackInventory<Y>)container!;
        }

        public virtual IStackInventory<Y> ShuffledItemsContainer(int size, int stackSize, params Y[] items)
        {
            if (items.Length > size * stackSize)
            {
                throw new ArgumentException($"Item amount ({items.Length}) cannot be bigger than the container size ({size})");
            }

            var containerType = GetInventoryType();

            var slotType = GetSlotTypeFromConstructor();

            Array slots = Array.CreateInstance(slotType, size);
            for (int index = 0; index < size; index++)
            {
                if (index < items.Length)
                {
                    slots.SetValue(
                        slotFactory.FullSlot(items[index], stackSize),
                        index
                    );
                }
                else
                {
                    slots.SetValue(
                        slotFactory.EmptySlot(stackSize),
                        index
                    );
                }
            }
            slots.ShuffleItems();

            var container = Activator.CreateInstance(containerType, slots);

            return (IStackInventory<Y>)container!;
        }
    }
}
