using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Controllers.Characters.Vergen
{
    public class VergenGhostTrigger : MonoBehaviour
    {
        public bool isRightSide;
        public VergenGhostTrigger oppositeSide;
        public GameObject vergenGhost;
        public GameObject VergenHeadAsesina;
        public float Speed = 30;
        private bool canTransition;

        private GameObject currentVergenGhost;

        public UnityEvent OnVergenGhostReachedEnd;
        public UnityEvent OnVergenHeadDodged;

        private void Awake()
        {
            if (OnVergenGhostReachedEnd == null)
                OnVergenGhostReachedEnd = new UnityEvent();
            if (OnVergenHeadDodged == null)
                OnVergenHeadDodged = new UnityEvent();
        }

        private void Start()
        {
            oppositeSide.OnVergenGhostReachedEnd.AddListener(() =>
            {
                canTransition = false;
                Destroy(currentVergenGhost);
                StartCoroutine(oppositeSide.SpawnHeadAsesina());
            });
        }

        private void Update()
        {
            if (canTransition && currentVergenGhost != null)
            {
                currentVergenGhost.transform.position = currentVergenGhost.transform.position + new Vector3(Speed * Time.deltaTime, 0)  * currentVergenGhost.transform.right.x;
            }
        }

        public void Run()
        {
            if (isRightSide)
            {
                currentVergenGhost = Instantiate(vergenGhost, transform.position, Quaternion.Euler(0, 180, 0), transform);
            }
            else
            {
                currentVergenGhost = Instantiate(vergenGhost, transform.position, Quaternion.Euler(0, 0, 0), transform);
            }
            canTransition = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("VergenGhost"))
            {
                OnVergenGhostReachedEnd.Invoke();
                Destroy(other.transform.gameObject);
            }else if (other.transform.CompareTag("VergenHead"))
            {
                OnVergenHeadDodged.Invoke();
                Destroy(other.transform.gameObject);
            }
        }

        public IEnumerator SpawnHeadAsesina()
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
            
            if (isRightSide)
            {
                currentVergenGhost = Instantiate(VergenHeadAsesina, transform.position, Quaternion.Euler(0, 180, 0), transform);
            }
            else
            {
                currentVergenGhost = Instantiate(VergenHeadAsesina, transform.position, Quaternion.Euler(0, 0, 0), transform);
            }
            
            yield return 0;
        }
    }
}