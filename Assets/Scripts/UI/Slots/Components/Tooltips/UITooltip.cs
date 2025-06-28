using UnityEngine;
using UnityEngine.UI;
using TheChest.Examples.Items;

namespace TheChest.Slots.UI.Components.Tooltips
{
    public class UITooltip : MonoBehaviour
    {
        [SerializeField]private Image image;
        [SerializeField]private Text title;
        [SerializeField]private Text description;

        public void ShowItem(Item item)
        {
            this.image.sprite       = item.Image;
            this.title.text         = SanitizeTitle(item.Name);
            this.description.text   = SanitizeDescription(item.Description);
        }

        #region Sanitize text
        private const int maxTitleSize = 30;
        private const int maxDescriptionSize = 84;

        private static string Sanitize(string str, int size)
        {
            if (str?.Length <= size)
            {
                return str;
            }
            else
            {
                return str?.Substring(0, size - 3) + "...";
            }
        }

        private static string SanitizeTitle(string str)
        {
            return Sanitize(str,maxTitleSize);
        }

        private static string SanitizeDescription(string str)
        {
            return Sanitize(str, maxDescriptionSize);
        }
        #endregion
    }
}