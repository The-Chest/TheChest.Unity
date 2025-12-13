# Implementing a custom IStackInventory<T>
You can implement the interface `IStackInventory<T>` directly if you need more control.

## IStackInventory

```csharp
public class MyStackInventory : IStackInventory<int>{
    protected readonly IInventoryStackSlot<int>[] slots;

    public MyStackInventory(IInventoryStackSlot<int>[] slots)
    {
        this.slots = slots;
    }

    public bool Add(int item){
        if(item < 1)
            return false;

        if(this.slot[item].IsFull)
            return false;

        for(int i = 0; i < this.slot[item].Content.Length; i++){
            if(this.slot[item].Content[i] <0 || this.slot[item].Content[i] == null){
                this.slot[item].Content[i] = item;
                return true;
            }
        }

        return false;
    }
    /// all other methods will need to be implemented too
}
```

## IInventoryStackSlot

```csharp
public class MyStackSlot : IInventoryStackSlot<int>
{
    protected readonly int[] contents;
    public int Content[] => this.contents;
    public bool IsFull => this.Content > 0;

    public MyStackSlot(int[] contents)
    {
        this.contents = contents;
    }

    public virtual bool Add(int item)
    {
        if(item <= 0 || this.IsFull)
            return false;

        for (int i = 0; i < this.contents; i++)
        {
            if (this.contents[i] == 0)
            {
                this.contents[i] += item;
                return true;
             }
        }
        return false;
    }
    /// all other methods will need to be implemented too
}
```