using Items;
using UnityEngine;

namespace Inventory
{
    public class InventorySlotManager : MonoBehaviour
    {
        private Item currentItem;

        public void AddItem(Item item)
        {
            if(currentItem != null)
                currentItem.Eject();
            GameManager.Instance.GetHUD().AddItem(item.Kind);
            currentItem = item;
        }
        
        public void RemoveItem()
        {
            if (currentItem != null)
            {
                currentItem.Eject();
                currentItem = null;
            }
        }

        public void UseItem()
        {
            if (currentItem != null)
            {
                currentItem.Use();
                RemoveItem();
                GameManager.Instance.GetHUD().RemoveItem();
            }
        }
    }
}