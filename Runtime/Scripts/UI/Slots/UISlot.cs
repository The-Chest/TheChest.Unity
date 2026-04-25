using System;
using UnityEngine;
using UnityEngine.UI;
using TheChest.Examples.Containers;
using TheChest.Examples.Items;
using System.Linq;

namespace TheChest.Slots.UI
{
    /// <summary>
    /// UI Slot with name, amount and icon
    /// </summary>
    [DisallowMultipleComponent]
    public class UISlot : MonoBehaviour
    {
        #region UI
        [Header("Values")]
        [Tooltip("The Image element wich will render the item sprite")]
        [SerializeField] private Image itemSprite;

        [SerializeField] private StackSlot slot;

        [Tooltip("The Text element wich will render the item amount")]
        [SerializeField] private Text itemAmount;
        #endregion

        #region Properties
        public Image ItemSprite => itemSprite;

        public int Index { get; protected set; } 
        public int Amount { get; protected set; }

        public bool IsEmpty => this.Amount == 0;

        public event Action<int,int> OnSelectIndex;
        #endregion

        #region Interface Implementations
        public StackSlot Slot => this.slot;

        public void Select()
        {
            this.OnSelectIndex?.Invoke(this.Index, this.Amount);
        }

        public void SetSlot(StackSlot slot, int slotIndex)
        {
            this.Index = slotIndex;
            this.Amount = slot.Amount;
            this.slot = slot;

            this.SetItem(slot.Content!.FirstOrDefault());
        }

        public void Refresh(StackSlot slot, bool selected = false)
        {
            this.Amount = slot.Amount;
            this.slot = slot;

            this.SetItem(slot.Content!.FirstOrDefault());
            
            this.ChangeSelected(selected);
        }
        #endregion

        #region Private methods
        private void ChangeSelected(bool selected)
        {
            if (selected)
            {
                this.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                this.GetComponent<Image>().color = Color.white;
            }
        }

        private void SetItem(Item item)
        {
            if (!slot.IsEmpty && !(item is null))
            {
                this.itemAmount.text = slot.Amount == 0 ? string.Empty : slot.Amount.ToString();
                this.itemSprite.sprite = item.Image;
                this.itemSprite.color = Color.white;
            }
            else
            {
                this.itemAmount.text = string.Empty;
                this.itemSprite.sprite = null;
                this.itemSprite.color = new Color(0, 0, 0, 0);
            }
        }
        #endregion
    }
}
