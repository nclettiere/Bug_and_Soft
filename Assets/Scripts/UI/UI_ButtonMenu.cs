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
    private bool isMainMenu;
    
    [SerializeField]
    private AudioSource selectorChangeSFX;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isMainMenu && GameManager.Instance.GetMainMenuOn())
        {
            selector.transform.position = transform.position;
            selectorChangeSFX.Play();
        }else if (!isMainMenu && !GameManager.Instance.GetMainMenuOn())
        {  
            selector.transform.position = transform.position;
            selectorChangeSFX.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
