using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SlotButton : 
	MonoBehaviour, 
	IPointerEnterHandler,
	IPointerExitHandler
{
	[SerializeField]
	private GameObject selectorKnob;
	[SerializeField]
	private GameObject slotSaveLoadPanel;
	[SerializeField]
	private GameObject slotSaveLoadButtons;
	
	[SerializeField]
	private Animator[] slotsAnimator;
	
	[SerializeField]
	private Button returnButton;
	[SerializeField]
	private UI_PauseMenuRemastered pauseMenu;
    
    [SerializeField]
    private AudioSource selectorChangeSFX;
    
    [SerializeField]
    private float knobPositionY;

    public void OnPointerEnter(PointerEventData eventData)
    {
		Debug.Log("OnPointerEnter");
    	selectorKnob.SetActive(true);
        selectorKnob.transform.position = new Vector3(selectorKnob.transform.position.x, transform.position.y, selectorKnob.transform.position.z);
        //selectorChangeSFX.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    	selectorKnob.SetActive(false);
    }

    public void OnClick()
    {
	    
	    slotsAnimator[0].SetBool("Collapse", true);
	    slotsAnimator[1].transform.gameObject.GetComponent<Button>().interactable = false;
	    slotsAnimator[2].transform.gameObject.GetComponent<Button>().interactable = false;
	    
	    returnButton.onClick.RemoveAllListeners();
	    returnButton.onClick.AddListener(delegate
	    {
		    slotSaveLoadPanel.SetActive(false);
		    slotSaveLoadButtons.SetActive(false);
		    
		    slotsAnimator[0].SetBool("Collapsed", false);
		    slotsAnimator[1].transform.gameObject.GetComponent<Button>().interactable = true;
		    slotsAnimator[2].transform.gameObject.GetComponent<Button>().interactable = true;
		    
		    returnButton.onClick.RemoveAllListeners();
		    returnButton.onClick.AddListener(delegate
		    {
			    pauseMenu.CloseLoadMenu();
		    });
	    });
	    
	    slotSaveLoadPanel.SetActive(true);
		slotSaveLoadButtons.SetActive(true);
    }

    public void AnimNotifyCollapsedEnded()
    {
	    slotsAnimator[0].SetBool("Collapse", false);
	    slotsAnimator[0].SetBool("Collapsed", true);
    }
}
