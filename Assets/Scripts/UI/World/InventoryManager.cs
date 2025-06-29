using TheChest.Containers.UI;

namespace TheChest.World
{
    public static class InventoryManager
    {
        private static UIInventory playerInventory;

        public static UIInventory PlayerInventory { 
            get => playerInventory;
            set {
                if(playerInventory == null)
                {
                    playerInventory = value;
                }
            }
        }
    }
}
