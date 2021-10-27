using System.Collections;
using Controllers.Damage;
using Player;
using UnityEngine;

namespace Controllers.Characters.Vergen
{
    public class VergenSpear : MonoBehaviour
    {
        private float speed;
        private float distance;
        private float xStartPos;
        [SerializeField] private float gravity;
        private Rigidbody2D rb;
        private bool isGravityOn;
        private bool hasHitGround;
        private bool playerRolled;
        private bool isVertical;

        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private Transform damagePos;
        [SerializeField] private float damageRadius;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            if (!isVertical)
                rb.velocity = transform.right * speed;
            else
                rb.velocity = Vector2.down * speed;

            xStartPos = transform.position.x;
            isGravityOn = false;
        }

        private void Update()
        {
            if (!hasHitGround || playerRolled)
            {
                if (isGravityOn)
                {
                    float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!hasHitGround)
            {
                Collider2D damageHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsPlayer);
                Collider2D groundHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsGround);

                if (damageHit)
                {
                    PlayerController pCtrl = damageHit.transform.GetComponent<PlayerController>();
                    DamageInfo damageInfo = new DamageInfo(15, transform.position.x, true);
                    pCtrl.Damage(damageInfo);

                    Destroy(gameObject);
                }

                if (groundHit)
                {
                    hasHitGround = true;
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;
                    
                    StartCoroutine(DestroyDelayed());
                }

                if (Mathf.Abs(xStartPos - transform.position.x) >= distance && !isGravityOn)
                {
                    isGravityOn = true;
                    rb.gravityScale = gravity;
                }
            }
        }

        public void FireProjectile(float speed, float distance, bool isVertical = false)
        {
            this.speed = speed;
            this.distance = distance;
            this.isVertical = isVertical;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(damagePos.position, damageRadius);
        }
        
        private IEnumerator DestroyDelayed() 
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            Destroy(gameObject);
        }
    }
}