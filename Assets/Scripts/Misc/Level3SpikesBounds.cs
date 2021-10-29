using System;
using UnityEngine;

namespace Misc
{
    public class Level3SpikesBounds : MonoBehaviour
    {
        private Animator animator;
        
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void BoundsOn()
        {
            animator.SetBool("Off", false);
            animator.SetBool("On", true);
        }
        
        public void BoundsOff()
        {
            animator.SetBool("On", false);
            animator.SetBool("Off", true);
        }
    }
}