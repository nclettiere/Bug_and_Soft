using System;
using UnityEngine;

namespace Misc
{
    public class RotateInCentre : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float angle = 20;
        private void Update()
        {
            transform.RotateAround(target.position, target.transform.forward, angle * Time.deltaTime * 10);
        }
    }
}