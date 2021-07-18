using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("Camera Options")]
    [SerializeField] private bool ChangeCameraOffset = false;
    [SerializeField] private bool ChangeCameraBounds = false;
    [SerializeField] private bool ChangeCameraSize = false;

    [Space]


    [Header("Camera Values")]
    [SerializeField] private Vector2 CameraOffsets = new Vector2(1.3f, 0f);
    [SerializeField] private float CameraBounds = 0.5f;
    [SerializeField] private float CameraSize = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If collision parent is in layer 6 (PLAYER)
        if(collision.gameObject.layer == 6)
        {
            if(ChangeCameraOffset)
            {
                GameManager.Instance.SetCameraOffset(CameraOffsets);
            }
        }
    }
}
