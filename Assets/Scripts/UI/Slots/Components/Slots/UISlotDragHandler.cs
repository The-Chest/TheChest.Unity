using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheChest.Slots.UI.Components.Slots
{
    /// <summary>
    /// Class to handle Drag'n drop on Slot
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UISlotDragHandler : UISlotComponent, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        private static Image image;

        new void Start()
        {
            base.Start();
            if (image == null)
            {
                image = Instantiate(new GameObject("SelectedItemSprite").AddComponent<Image>(), slot.transform.parent.parent);
                image.rectTransform.sizeDelta = slot.ItemSprite.rectTransform.sizeDelta;
                image.raycastTarget = false;
                image.enabled = false;
            }
        }
        /// <summary>
        /// Event that occour when the slot is clicked
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left && !slot.IsEmpty)
            {
                image.enabled = true;
                image.sprite = slot.ItemSprite.sprite;
                slot.Select();
            }
        }

        /// <summary>
        /// Event that occour when the item is beeing dragging
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !slot.IsEmpty)
            {
                image.rectTransform.position = Input.mousePosition;
            }
        }

        /// <summary>
        /// Event that occour when the item has stopped to be dragged 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            image.enabled = false;
            image.rectTransform.position = slot.ItemSprite.rectTransform.position;
        }

        public void OnDrop(PointerEventData eventData)
        {
            slot.Select();
        }
    }
}
