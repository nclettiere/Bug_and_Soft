using System;
using UnityEngine;

namespace Misc
{
    public class EvigBrannAltar : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                GameManager.Instance.LoadCutscene(0);
            }
        }
    }
}