using Items;
using UnityEngine;

namespace Inventory
{
    public class InventorySlotManager : MonoBehaviour
    {
        private Item currentItem;
        private bool itemEjected;

        public void AddItem(Item item)
        {
            if(currentItem != null)
                currentItem.Eject();
            
            GameManager.Instance.GetHUD().AddItem(item.Kind);
            currentItem = item;
            itemEjected = false;
        }
        
        public void RemoveItem()
        {
            if (currentItem != null)
            {
                currentItem.Eject();
                currentItem = null;
                itemEjected = true;
            }
        }

        public void UseItem()
        {
            if (!itemEjected)
            {
                currentItem.Use();
                RemoveItem();
                GameManager.Instance.GetHUD().RemoveItem();
                itemEjected = true;
            }
        }
    }
}