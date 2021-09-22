using UnityEngine;

namespace Items
{
    public class KroneItem : Item
    {
        [SerializeField] private int quantity;
        
        public override void Add()
        {
            isSpawned = false;
            GameManager.Instance.AddPlayerKrowns(quantity);
            Destroy(gameObject);
        }
    }
}