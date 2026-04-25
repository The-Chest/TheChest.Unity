# TheChest.Inventories

TheChest.Inventories is a library designed to manage inventories and slots for generic item collections. It provides a flexible and extensible framework for handling inventory systems, including support for stacked items, slot-based management.

## How does it work

The library provides a robust framework for managing inventories and slots in a generic and extensible way. It is designed to handle various inventory operations, such as adding, removing, moving, and retrieving items, while supporting both single-item slots and stackable items.

### Features

- **Generic Inventory Management**: Supports generic item types for flexibility.
- **Slot-Based System**: Items are stored and managed in slots, with interfaces and implementations for inventory slots.
- **Extensible Interfaces**: Allows for custom implementations and extensions of the inventory system.
- **Core Operations**: Add, retrieve and move  items from inventories and slots.
- **Error Handling**: Includes exception handling for invalid operations.

## Project Structure

### Inventory
- **`Inventory<T>`** : A generic inventory implementation that manages items in slots. 
    - **`InventorySlot<T>`**: Represents a single slot in the inventory.
- **`StackInventory<T>`** : A generic inventory that manages stackable items.
    - **`InventoryStackSlot<T>`**: Represents a slot that can hold multiple items of the same type.
- **`LazyStackInventory<T>`** : A generic stack inventory that allows lazy loading of items.
    - **`LazyInventoryStackSlot<T>`**: Represents a slot that can hold one item with amount deciding how much it can return.

## How to use it

### Installation

#### NuGet
To install the library via NuGet, you need to add a NuGet package origin reference to the following URL:
```
https://nuget.pkg.github.com/The-Chest/index.json
```
After adding the reference, use the following command to install the package:
```bash
nuget install TheChest.Inventories
```

#### DLL
Alternatively, you can download the DLL file and reference it directly in your project.

## Usage
The library provides ready-to-use implementations such as `Inventory<T>` and `InventorySlot<T>`. These can be used directly. For example:

### Inventory
```csharp
var slots = new IInventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>();
}

var inventory = new Inventory<string>(slots);
inventory.Add("Item1");
```

### StackInventory
```csharp
var stackSlots = new IInventoryStackSlot<string>[10];
for (int i = 0; i < stackSlots.Length; i++)
{
    stackSlots[i] = new InventoryStackSlot<string>(new string[0], 5);
}

var stackInventory = new StackInventory<string>(stackSlots);
stackInventory.Add("StackableItem", "StackableItem");
``` 

### LazyStackInventory
```csharp
var lazyStackSlots = new IInventoryLazyStackSlot<string>[10];
for (int i = 0; i < lazyStackSlots.Length; i++)
{
    lazyStackSlots[i] = new InventoryLazyStackSlot<string>($"item_{i}_",5 , 2);
}

var lazyStackInventory = new LazyStackInventory<string>(lazyStackSlots);
stackInventory.Add("StackableItem", "StackableItem");
```

### Creating a custom Inventory
If you need more control, you can implement the interfaces or extend classes.

- **`Inventory<T>`**
    - [docs/inventory/implementing](/docs/inventory/implementing.md)
    - [docs/inventory/extending](/docs/inventory/extending.md)
- **`StackInventory<T>`**
    - [docs/stack_inventory/implementing](/docs/stack_inventory/implementing.md)
    - [docs/stack_inventory/extending](/docs/stack_inventory/extending.md)
- **`LazyStackInventory<T>`**
    - [docs/lazy_stack_inventory/implementing](/docs/lazy_stack_inventory/implementing.md)
    - [docs/lazy_stack_inventory/extending](/docs/lazy_stack_inventory/extending.md) 

## Docs 

- **`Inventory<T>`**
    - [docs/inventory/class_diagram](/docs/inventory/class_diagram.md)
    - [docs/inventory/events](/docs/inventory/events.md)
- **`StackInventory<T>`**
    - [docs/stack_inventory/class_diagram](/docs/stack_inventory/class_diagram.md)
    - [docs/stack_inventory/events](/docs/stack_inventory/events.md)
- **`LazyStackInventory<T>`**
    - [docs/lazy_stack_inventory/class_diagram](/docs/lazy_stack_inventory/class_diagram.md)
    - [docs/lazy_stack_inventory/events](/docs/lazy_stack_inventory/events.md)

## Future Plans

The plans for future versions of the project are in this [GitHub Project Board](https://github.com/orgs/The-Chest/projects/19/views/2).
