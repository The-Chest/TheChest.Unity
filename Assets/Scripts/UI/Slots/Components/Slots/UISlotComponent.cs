using UnityEngine;

namespace TheChest.Slots.UI.Components.Slots
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(UISlot))]
    public abstract class UISlotComponent : MonoBehaviour
    {
        public UISlot slot {
            get;
            protected set;
        }

        public void Start()
        {
            slot = this.GetComponent<UISlot>();
        }
    }
}