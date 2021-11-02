using System;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class InventorySlotManager : MonoBehaviour
    {
        private Item currentItem;
        private bool itemEjected;
        
        private bool firstTimeHealing = true;
        
        public UnityEvent OnFirstHeal;

        private void Awake()
        {
            if (OnFirstHeal == null)
                OnFirstHeal = new UnityEvent();
        }

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
            GameManager.Instance.GetHUD().RemoveItem();
        }

        public void UseItem()
        {
            if (!itemEjected)
            {
                if (currentItem.Kind == EItemKind.HEALTH_POTION && firstTimeHealing)
                {
                    OnFirstHeal.Invoke();
                    firstTimeHealing = false;
                }
                currentItem.Use();
                RemoveItem();
                GameManager.Instance.GetHUD().RemoveItem();
                itemEjected = true;
            }
        }

    }
}