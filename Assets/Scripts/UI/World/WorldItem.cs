using TheChest.Examples.Items;
using TheChest.World;
using UnityEngine;

/// <summary>
/// Example of world item
/// </summary>
public class WorldItem : MonoBehaviour
{
    public Item Item {
        get => item;
        set {
            item = value;
        }
    }
    [SerializeField] private Item item;

    public int Amount {
        get => amount;
        set {
            if (value <= 0)
            {
                value = 1;
            }
            this.amount = value;
        }
    }
    [Range(1,100)]
    [SerializeField] private int amount;

    private void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = item?.Image;
    }

    public void OnMouseDown()
    {
        if (InventoryManager.PlayerInventory.Add(item, amount))
        {
            Destroy(this.gameObject);
        }
    }
}
