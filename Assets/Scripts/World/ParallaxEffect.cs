using System;
using UnityEngine;

namespace World
{
    public class ParallaxEffect : MonoBehaviour
    {
        private Transform cameraTransform;
        private Vector3 lastCameraPos;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            lastCameraPos = cameraTransform.position;
        }

        private void LateUpdate()
        {
            Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
            transform.position = deltaMovement;
            lastCameraPos = cameraTransform.position;
        }
    }
}