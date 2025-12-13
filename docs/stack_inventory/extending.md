# Extending bult-in Inventory
You can extend the built-ind class `StackInventory<T>` to override with your own features.

## StackInventory
```csharp
public class MyStackInventory : StackInventory<int>
{
    public MyStackInventory(MyInventoryStackSlot[] slots, int maxStackAmount) : base(slots, maxStackAmount)
    {
        if (maxStackAmount < 1)
            throw new System.ArgumentException("Invalid inventory maxStackAmount");
    }

    public override bool CanAdd(int item)
    {
        if(item <= 0)
            return false;
        
        return base.CanAdd(item);
    }

    // You can override other methods as needed
}
```

## InventoryStackSlot

```csharp
public class MyInventoryStackSlot : InventoryStackSlot<int>
{
    private int amount;
    public MyInventoryStackSlot(int[] items) : base(items) { 
        if (items.Length != 10)
            throw new System.ArgumentException("Invalid slot size");

        amount = items.Length;
    }

    public override int? Get()
    {
        if (this.amount <= 0 || this.IsEmpty)
        {
            return 0;
        }
        amount--;
        return base.Get();
    }

    // You can override other methods as needed
}
```