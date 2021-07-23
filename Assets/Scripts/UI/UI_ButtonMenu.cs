using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonMenu :
    /// @cond SKIP_THIS
    MonoBehaviour,
    /// @endcond
    IPointerEnterHandler,
    IPointerExitHandler
{

    [SerializeField]
    private GameObject selector;

    [SerializeField]
    private AudioSource selectorChangeSFX;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.GetMainMenuOn())
        {
            selector.transform.position = transform.position;
            selectorChangeSFX.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
