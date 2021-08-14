using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpMarker : MonoBehaviour
{
    private float destroyWaitTime = 5f;

    private float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        destroyTime = Time.time + destroyWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}