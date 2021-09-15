using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class ScreenOpts : MonoBehaviour
{
    public LocalizeStringEvent localizeString;
    public static string sliderFPSamountSTR = "60FPS";
    public static string sliderVolumeStr = "0.2";
    
    internal void SetFPSStr(int sliderFPSamount)
    {
        if (sliderFPSamount < 241)
        {
            sliderFPSamountSTR = sliderFPSamount + "FPS";
        }
        else
        {
            sliderFPSamountSTR = "NO LIMIT";
        }

        localizeString.StringReference.RefreshString();
    }

    internal void SetVolumeStr(float sliderVolumeAmount)
    {
        sliderVolumeStr = sliderVolumeAmount.ToString("0.0");
        localizeString.RefreshString();
        localizeString.StringReference.RefreshString();
    }
}
