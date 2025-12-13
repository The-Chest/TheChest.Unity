# Implementing a custom ILazyStackInventory<T>
You can implement the interface `ILazyStackInventory<T>` directly if you need more control.

## ILazyStackInventory<T>

```csharp
public class MyLazyStackInventory : ILazyStackInventory<int>
{
    protected readonly IInventoryLazyStackSlot<int>[] slots;

    public int Size => this.slots.Length;

    public MyLazyStackInventory(IInventoryLazyStackSlot<int>[] slots)
    {
        this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
    }
    
    public virtual bool Add(int item)
    {
        if(item == 0)
            return false;

        for (int index = 0; index < this.Size; index++)
        {
            var slot = this.slots[index];
            if (slot.CanAdd(item))
            {
                return slot.Add(item) == 0;
            }
        }

        return false;
    }
    /// all other methods will need to be implemented too
}
```

## IInventoryStackLazySlot

```csharp
public class MySlot : InventoryLazyStackSlot<int>
{
    public int Content { get; private set; }
    public int StackAmount { get; private set; }
    public int MaxStackAmount { get; private set; }
    public bool IsFull => this.content > 0 || this.StackAmount == this.MaxStackAmount;

    public InventoryLazyStackSlot(int item, int amount, int maxStackAmount) 
    {
        this.Content = item;
        this.StackAmount = amount;
        this.MaxStackAmount = maxStackAmount;
    }

    public bool CanAdd(int item)
    {
        if (item == 0)
            return false;

        if (this.IsFull || this.Size > 0)
            return false;

        return true;
    }
    /// all other methods will need to be implemented too
}
```