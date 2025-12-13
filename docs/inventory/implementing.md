# Implementing a custom IInventory<T>
You can implement the interface `IInventory<T>` directly if you need more control.

## IInventory<T>

```csharp
public class MyInventory : IInventory<int>{
    protected readonly IInventorySlot<int>[] slots;

    public MyInventory(IInventorySlot<int>[] slots)
    {
        this.slots = slots;
    }

    public bool Add(int item){
        if(item < 1)
            return false;

        if(this.slot[item].IsFull)
            return false;

        this.slot[item] = item;
        return true;
    }
    /// all other methods will need to be implemented too
}
```

## IInventorySlot<T>

```csharp
public class MySlot : IInventorySlot<int>
{
    public int Content { get; private set; } = 0;
    public bool IsFull => this.Content > 0;

    public virtual bool Add(T item)
    {
        if(this.IsFull){
            return false;
        }
        this.Content = item;
        return true;
    }

    /// all other methods will need to be implemented too
}
```