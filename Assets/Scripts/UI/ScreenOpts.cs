using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class ScreenOpts : MonoBehaviour
{
    public int currentFPS;
    public LocalizeStringEvent localizeString;
    public static string sliderFPSamountSTR;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentFPS = (int)(Time.frameCount / Time.time);
    }

    internal void SetFPSStr(int sliderFPSamount)
    {
        if (sliderFPSamount < 241)
        {
            sliderFPSamountSTR = sliderFPSamount.ToString() + "FPS";
        }
        else
        {
            sliderFPSamountSTR = "NO LIMIT";
        }

        localizeString.StringReference.RefreshString();
    }
}
