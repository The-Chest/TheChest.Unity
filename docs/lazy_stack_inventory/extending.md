# Extending bult-in Inventory
You can extend the built-ind class `LazyStackInventory<T>` to override with your own features.

## LazyStackInventory<T>

```csharp
public class MyLazyStackInventory : LazyStackInventory<int>
{
    public MyLazyStackInventory(MyInventoryLazyStackSlot[] slots) : base(slots) { }

    public override bool CanAdd(int item)
    {
        if(item <= 0)
            return false;
        
        return base.CanAdd(item);
    }

    // You can override other methods as needed
}
```

## InventoryLazyStackSlot<T>

```csharp
public class MyInventoryLazyStackSlot : InventoryLazyStackSlot<int>
{
    public MyInventoryLazyStackSlot(int item, int amount, int maxAmount) : base(item, amount, maxAmount) { 
        if (amount > maxAmount)
            throw new System.ArgumentException("Amount cannot be bigger than maxAmount");
    }

    public override int? Get()
    {
        if (this.StackAmount <= 0 || this.IsEmpty)
        {
            return 0;
        }
        var result = base.Get();

        if (!result.HasValue)
        {
            return 0;
        }

        return result;
    }

    // You can override other methods as needed
}
```