using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TheChest.Containers.UI.Components
{
    /// <summary>
    /// Inventory Drop Area
    /// </summary>
    public class DropArea : MonoBehaviour, IDropHandler
    {
        /// <summary>
        /// Event dispatch when <see cref="DropArea.OnDrop(PointerEventData)"/> is called 
        /// </summary>
        public Action OnDropItem;

        public void OnDrop(PointerEventData eventData)
        {
            this.OnDropItem?.Invoke();
        }
    }
}
