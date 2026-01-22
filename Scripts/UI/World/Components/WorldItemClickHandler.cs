using UnityEngine;
using UnityEngine.EventSystems;

namespace TheChest.World.Components
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(WorldItem))]
    public class WorldItemClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [System.Obsolete]
        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                var item = this.GetComponent<WorldItem>().Item;
                var amount = this.GetComponent<WorldItem>().Amount;
                if (InventoryManager.PlayerInventory.Add(item,amount))
                {
                    Destroy(this.gameObject);
                }
            }
        }

        [System.Obsolete]
        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        [System.Obsolete]
        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
