using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelOpacity : MonoBehaviour
{
    public void FadePanel(bool fadeMode)
    {
        var panelSettingsGroup = GetComponent<CanvasGroup>();
        float end = fadeMode ? 1 : 0;
        StartCoroutine(DoFade(panelSettingsGroup, panelSettingsGroup.alpha, end, 1));
    }

    public IEnumerator DoFade(CanvasGroup cg, float start, float end, float duration)
    {
        float counter = 0f;

        while(counter < duration)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    }
}
