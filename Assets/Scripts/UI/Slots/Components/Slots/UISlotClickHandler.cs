using UnityEngine;
using UnityEngine.EventSystems;

namespace TheChest.Slots.UI.Components.Slots
{
    /// <summary>
    /// Class to handle Click On Slot
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UISlotClickHandler : UISlotComponent, IPointerClickHandler 
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                this.slot.Select();
            }
        }
    }
}
