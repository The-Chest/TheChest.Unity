using TheChest.Containers.Generics.Interfaces;
using TheChest.Slots.Generics.Interfaces;

namespace TheChest.Containers.Generics.Base
{
    public abstract class BaseStackContainer<T> : BaseContainer<T>, IStackContainer<T>
    {
        private IStackSlot<T>[] slots => this.Slots as IStackSlot<T>[];
    }
}
