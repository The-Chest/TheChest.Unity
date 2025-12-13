# LazyStackInventory events

## LazyStackInventoryGetEventHandler

Fires when an item is returned from the inventory.

### Signature
```csharp
public delegate void LazyStackInventoryGetEventHandler<T>(object? sender, LazyStackInventoryGetEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                           | Description                                        |
|---------------------------|----------------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                                       | Inventory responsible for firing the event         |
| args                      | `LazyStackInventoryGetEventArgs`                               | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>>`   | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                                      | Found item                                         |
| args.Data[].Index         | `Integer`                                                      | Index number where the `Item` was found            |
| args.Data[].Amount        | `Integer`                                                      | Amount of `Item` found                             |

### Example

```csharp
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventoryLazyStackSlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}", 5, 10);
}

var inventory = new LazyStackInventory<string>(slots);
inventory.OnGet += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"{action.Amount} amount of the Item {action.Item} returned from index {action.Index}");
    }
};

//Any method except GetCount will fire the event
var result = inventory.Get(0);
Console.WriteLine($"{result.Length} amount of the Item {result} returned from index {0}");
```

## LazyStackInventoryAddEventHandler

Fires when any amount of item is added to a slot on the inventory.

### Signature
```csharp
public delegate void LazyStackInventoryAddEventHandler<T>(object? sender, LazyStackInventoryAddEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                           | Description                                        |
|---------------------------|----------------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                                       | Inventory responsible for firing the event         |
| args                      | `LazyStackInventoryAddEventArgs`                               | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<LazyStackInventoryAddEventArgs<T>>`       | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                                      | Added item                                         |
| args.Data[].Index         | `Integer`                                                      | Index number where the `Item` was added            |
| args.Data[].Amount        | `Integer`                                                      | Amount of `Item` added                             |

### Example

```csharp
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventoryLazyStackSlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}", 5, 10);
}

var inventory = new LazyStackInventory<string>(slots);
inventory.OnAdd += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"{action.Amount} amount of the Item {action.Item} added to the index {action.Index}");
    }
};

//Any method except GetCount will fire the event
var result = inventory.AddAt("item_10", 0);
Console.WriteLine($"{result.Length} amount of the Item {result} added to the index {0}");
```

## LazyStackInventoryMoveEventHandler

Fires when any amount of item is moved to a slot on the inventory.

### Signature
```csharp
public delegate void LazyStackInventoryMoveEventHandler<T>(object? sender, LazyStackInventoryMoveEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                          | Description                                          |
|---------------------------|---------------------------------------------------------------|------------------------------------------------------|
| sender                    | `object`                                                      | Inventory responsible for firing the event           |
| args                      | `LazyStackInventoryMoveEventArgs`                             | Class that holds data of the event                   |
| args.Data                 | `IReadOnlyCollection<LazyStackInventoryMoveItemEventData<T>>` | An array with all items and its respective indexes   |
| args.Data[].Item          | `Array.Generic`                                               | Item moved                                           |
| args.Data[].FromIndex     | `Integer`                                                     | Index number where the `Item` is originally from     |
| args.Data[].ToIndex       | `Integer`                                                     | Index number where the `Item` was moved              |
| args.Data[].Amount        | `Integer`                                                     | Amount of `Item` moved from origin to target index   |

### Warning
* It fires an event for each item that was moved.
* If the `FromIndex` is the same as the `ToIndex`, it will not fire the event.
* If there is no item in one of the indexes, it will not fire the event for that index.

### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventoryLazyStackSlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventoryLazyStackSlot<string>();
}

var inventory = new LazyStackInventory<string>(slots);
inventory.OnMove += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"{action.Amount} Items of {action.Item} were moved from the slot {action.FromIndex} to the slot {action.ToIndex}");
    }
};

inventory.Move(0, 1);//the method has no return
```
