# Events

## InventoryGetEventHandler

Fires when an item is returned to the inventory.

#### Signature
```csharp
public delegate void InventoryGetEventHandler<T>(object? sender, InventoryGetEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                  | Description                                        |
|---------------------------|-------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                              | Inventory responsible for firing the event         |
| args                      | `InventoryGetEventArgs`                               | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<InventoryGetItemEventData<T>>`   | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                             | Found item                                         |
| args.Data[].Index         | `Integer`                                             | Index number where the `Item` was found            |

### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>(slots);
inventory.OnGet += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"Item {action.Item} got from index {action.Index}");
    }
};

//Any method except GetCount will fire the event
var result = inventory.Get(0);
Console.WriteLine($"Item {result} got from index {0}");
```

## InventoryAddEventHandler

Fires when an item is added to the inventory.

### Signature
```csharp
public delegate void InventoryAddEventHandler<T>(object? sender, InventoryAddEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                  | Description                                        |
|---------------------------|-------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                              | Inventory responsible for firing the event         |
| args                      | `InventoryAddEventArgs`                               | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<InventoryAddItemEventData<T>>`   | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                             | Item added                                         |
| args.Data[].Index         | `Integer`                                             | Index number where the `Item` was added            |

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length - 2; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>(slots);
inventory.OnAdd += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"Item {action.Item} added on index {action.Index}");
    }
};

//Any Add method will fire the event (if sucessful)
var result = inventory.Add("Item");
Console.WriteLine($"Item {result} added");
```

## InventoryMoveEventHandler

Fires when an item is moved from an index to another to the inventory.

### Signature
```csharp
public delegate void InventoryMoveEventHandler<T>(object? sender, InventoryMoveEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                  | Description                                        |
|---------------------------|-------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                              | Inventory responsible for firing the event         |
| args                      | `InventoryMoveEventArgs`                              | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<InventoryMoveItemEventData<T>>`  | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                             | Item that were moved                               |
| args.Data[].FromIndex     | `Integer`                                             | Index number where the `Item` started              |
| args.Data[].ToIndex       | `Integer`                                             | Index number where the `Item` was moved to         |

### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length - 1; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>(slots);

inventory.OnMove += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"Item {action.Item} got from index {action.Index}");
    }
};

//This will fire an event with the two items moved
inventory.Move(0, 1);
//This will fire an event with the one item moved (the index 9 is empty)
inventory.Move(9, 2);
```