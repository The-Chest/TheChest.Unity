using UnityEngine;
using UnityEngine.EventSystems;
using TheChest.Slots.UI.Components.Tooltips;

namespace TheChest.Slots.UI.Components.Slots.Tooltips
{
    [DisallowMultipleComponent]
    public class UISlotTooltipHandler : UISlotComponent, IPointerEnterHandler, IPointerExitHandler
    {
        private static UITooltip tooltip;

        [SerializeField]private UITooltip tooltipPrefab;

        new void Start()
        {
            base.Start();
            if (tooltip == null)
            {
                tooltip = Instantiate(tooltipPrefab, slot.transform.parent.parent);
                tooltip.gameObject.SetActive(false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var tooltipRect = tooltip.GetComponent<RectTransform>();

            if (!this.slot.Slot.IsEmpty && Camera.current != null)
            {
                var slotRect = slot.GetComponent<RectTransform>();

                if (slotRect.transform.position.x > Camera.current.pixelWidth / 2)
                {
                    var horizontal = Vector3.left * (tooltipRect.rect.width + (slotRect.rect.width / 2));
                    var vertical = Vector3.down * (slotRect.rect.height / 2);
                    tooltipRect.localPosition = slotRect.localPosition + horizontal + vertical;
                }
                else
                {
                    var horizontal = Vector3.right * (slotRect.rect.width / 2);
                    var vertical = Vector3.down * (slotRect.rect.height / 2);
                    tooltipRect.localPosition = slotRect.localPosition + horizontal + vertical;
                }

                tooltip.gameObject.SetActive(true);
                tooltip.ShowItem(this.slot.Slot.CurrentItem);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}
