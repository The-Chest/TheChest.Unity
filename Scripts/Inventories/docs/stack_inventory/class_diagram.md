# Stack Inventory Diagram
The `StackInventory` class is a generic container that holds and manages items in slots that can hold more than one amount of the same type.

```mermaid
---
config:
  theme: mc
  look: classic
  class:
    hideEmptyMembersBox: true
---
classDiagram
direction TB
    namespace TheChest.Core{
        class StackContainer~T~ {
        }
    }
    
    namespace TheChest.Inventories{
        class StackInventory~T~ {
            - IInventoryStackSlot~T~[] slots
            + IInventoryStackSlot~T~ this[int index]
            + IInventoryStackSlot~T~[] Slots
            + StackInventory(IInventoryStackSlot~T~[] slots)
            + bool Add(T item)
            + T[] AddAt(T item, int index, bool replace = true)
            + bool AddAt(T item, int index)
            + T[] Add(params T[] items)
            + T[] AddAt(T[] items, int index, bool replace = true)
            + T[] AddAt(T[] items, int index)
            + T[] Clear()
            + T[] GetAll(int index)
            + T[] GetAll(T item)
            + T? Get(int index)
            + T? Get(T item)
            + T[] Get(T item, int amount)
            + T[] Get(int index, int amount)
            + int GetCount(T item)
            + void Move(int origin, int target)
        }    
        class IInventoryStackSlot~T~ {
            + bool CanAdd(T item)
            + bool CanAdd(T[] items)
            + void Add(ref T item)
            + void Add(ref T[] items)
            + bool CanReplace(T item)
            + bool CanReplace(T[] items)
            + T[] Replace(ref T[] items)
            + T[] Replace(ref T item)
            + T? Get()
            + T[] Get(int amount)
            + T[] GetAll()
            + bool Contains(T item)
        }
        class IStackInventory~T~ {
            + bool Add(T item)
            + bool AddAt(T item, int index)
            + T[] AddAt(T item, int index, bool replace = true)
            + T[] Add(params T[] items)
            + T[] AddAt(T[] items, int index)
            + T[] AddAt(T[] items, int index, bool replace = true)
            + T[] Clear()
            + T[] GetAll(int index)
            + T[] GetAll(T item)
            + T? Get(int index)
            + T? Get(T item)
            + T[] Get(T item, int amount)
            + T[] Get(int index, int amount)
            + int GetCount(T item)
            + void Move(int origin, int target)
        }
    }
	<<abstract>> StackContainer
	<<interface>> IStackInventory
	<<interface>> IInventoryStackSlot

    StackInventory ..|> IStackInventory
    StackInventory --> StackContainer
    IInventoryStackSlot --* StackInventory
```

## InventoryStackSlot Diagram
The `InventoryStackSlot` class can hold and manage a list of the same items inside it.

```mermaid
---
config:
  theme: mc
  look: classic
  class:
    hideEmptyMembersBox: true
---
classDiagram
direction BT

namespace TheChest.Core {
    class StackSlot~T~ {
        + T[] Content
        + int MaxStackAmount
        + int StackAmount
        + bool IsEmpty
        + bool IsFull
        + StackSlot(T[] items)
        + StackSlot(T[] items, int maxStackAmount)
    }
}
<<abstract>> StackSlot

namespace TheChest.Inventories {
    class IInventoryStackSlot~T~ {
        + bool CanAdd(T item)
        + bool CanAdd(T[] items)
        + void Add(ref T item)
        + void Add(ref T[] items)
        + bool CanReplace(T item)
        + bool CanReplace(T[] items)
        + T[] Replace(ref T item)
        + T[] Replace(ref T[] items)
        + T? Get()
        + T[] Get(int amount)
        + T[] GetAll()
        + bool Contains(T item)
    }

    class InventoryStackSlot~T~ {
        + InventoryStackSlot(T[] items)
        + InventoryStackSlot(T[] items, int maxStackAmount)
        + void Add(ref T item)
        + void Add(ref T[] items)
        + bool CanAdd(T item)
        + bool CanAdd(T[] items)
        + T[] GetAll()
        + T[] Get(int amount)
        + T? Get()
        + bool CanReplace(T item)
        + bool CanReplace(T[] items)
        + T[] Replace(ref T item)
        + T[] Replace(ref T[] items)
        + bool Contains(T item)
    }
}

<<interface>> IInventoryStackSlot
InventoryStackSlot~T~ --|>  StackSlot~T~
InventoryStackSlot~T~ ..|> IInventoryStackSlot~T~ 
```
