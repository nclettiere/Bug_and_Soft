using System;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        public EItemKind Kind;
        
        [SerializeField] private int lifetime = 20;

        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private int overlapRadius = 5;
        
        private float lifetimeWait;

        private Rigidbody2D rBody;

        private protected bool isSpawned;
        
        private protected virtual void Start()
        {
            lifetimeWait = Time.time + lifetime;
            // IMPULSE FROM THE GODS
            rBody = GetComponent<Rigidbody2D>();
            rBody.AddForce(new Vector2(10, 10), ForceMode2D.Impulse);
            isSpawned = true;
        }
        
        private protected virtual void Update()
        {
            if (GameManager.Instance.DeltaTime > 0f && isSpawned)
            {
                if (Time.time >= lifetimeWait)
                {
                    Destroy(gameObject);
                }
                
                CheckPlayerNearby();
            }
        }

        private protected virtual void CheckPlayerNearby()
        {
            var hit = Physics2D.OverlapCircle(transform.position, overlapRadius, whatIsPlayer);

            if (hit != null)
            {
                Add();
            }
        }
        
        public virtual void Eject(bool destroy = false)
        {
            if (destroy)
            {
                Destroy(gameObject);
            }
            else
            {
                Spawn(GameManager.Instance.PlayerController.transform.position);
            }
        }

        public virtual void Add()
        {
            isSpawned = false;
            GameManager.Instance.GetInventorySlotManager()
                .AddItem(this);
            Destroy(gameObject);
        }

        public virtual void Use()
        {
        }

        public virtual void Spawn(Vector3 position)
        {
            Instantiate(gameObject, position, Quaternion.Euler(0, 0, 0));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, overlapRadius);
        }
    }
}