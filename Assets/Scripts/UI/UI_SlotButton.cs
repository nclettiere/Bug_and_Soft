using SaveSystem.Data;
using TMPro;
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
	private int slotPosition;
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

    [SerializeField]
    private TextMeshProUGUI[] slotDataInfoText;
    
    [SerializeField]
    private GameObject emptySlotMesasge;

    public void OnPointerEnter(PointerEventData eventData)
    {
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

	    slotsAnimator[0].transform.gameObject.GetComponent<CanvasGroup>().alpha = 0;
	    slotsAnimator[0].transform.gameObject.GetComponent<Button>().interactable = false;
	    
	    slotsAnimator[1].transform.gameObject.GetComponent<Button>().interactable = false;
	    slotsAnimator[2].transform.gameObject.GetComponent<Button>().interactable = false;
	    
	    returnButton.onClick.RemoveAllListeners();
	    returnButton.onClick.AddListener(delegate
	    {
		    slotSaveLoadPanel.SetActive(false);
		    slotSaveLoadButtons.SetActive(false);
		    
		    slotsAnimator[0].transform.gameObject.GetComponent<CanvasGroup>().alpha = 1;
		    slotsAnimator[0].transform.gameObject.GetComponent<Button>().interactable = true;
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

    public void UpdateSlotInfoData()
    {
	    PlayerData playerData = SaveSystem.SaveSystem.LoadSaveGame(slotPosition);
	    
	    if (playerData != null)
	    {
		    foreach (var slotText in slotDataInfoText)
		    {
			    slotText.gameObject.SetActive(true);
		    }
		    emptySlotMesasge.SetActive(false);

		    string levelName = "Slot: " + slotPosition;

		    if (playerData.Level == 0)
		    {
			    levelName = "OLD VERGEN CHAMBER";
		    }

		    slotDataInfoText[0].text = levelName;
		    //slotDataInfoText[1].text = "Slot" + slotPosition;
		    slotDataInfoText[2].text = "HEALTH " + playerData.Health + "/250";
		    slotDataInfoText[3].text = "KRONES " + playerData.Krones;
	    }
	    else
	    {
		    foreach (var slotText in slotDataInfoText)
		    {
			    slotText.gameObject.SetActive(false);
		    }
		    emptySlotMesasge.SetActive(true);
	    }
    }

    public void AnimNotifyCollapsedEnded()
    {
	    slotsAnimator[0].SetBool("Collapse", false);
	    slotsAnimator[0].SetBool("Collapsed", true);
    }
}
