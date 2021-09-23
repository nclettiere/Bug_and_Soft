using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    public class UI_HoverEffect
        : MonoBehaviour,
            IPointerEnterHandler,
            IPointerExitHandler
    {
        [SerializeField] 
        private RectTransform elementToEffect;
        
        [SerializeField]
        private GameObject selector;

        [SerializeField]
        private bool isMainMenu;
        
        [SerializeField]
        private bool isLvlUpScreen;
        [SerializeField]
        private TextMeshProUGUI displayText;
        [SerializeField]
        private string whatToDisplay;
    
        [SerializeField]
        private AudioSource selectorChangeSFX;

        private RectTransform rTrans;
        private bool canTransition;
        Vector2 mousePos = Vector2.zero;
        private Vector2 elementSize;

        private void Start()
        {
            rTrans = transform.GetComponent<RectTransform>();
            elementSize =
                new Vector2(
                    transform.GetComponent<RectTransform>().rect.width,
                    transform.GetComponent<RectTransform>().rect.height);
        }

        private void Update()
        {
            if (canTransition)
            {
                mousePos.Set(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rTrans, mousePos, null, out var result))
                {
                    result += rTrans.sizeDelta / 2;

                    Vector2 normalized;
                    normalized.x = (2 * (result.x / elementSize.x) - 1) * -1;
                    normalized.y = (2 * (result.y / elementSize.y) - 1);

                    Quaternion q = Quaternion.Euler(40f * normalized.y, 40f * normalized.x, 0);
                    elementToEffect.rotation = Quaternion.Lerp(elementToEffect.rotation, q, Time.deltaTime * 5f);
                }
            }
            else
            {
                var rotZero = Quaternion.Euler(0, 0, 0);
                elementToEffect.rotation = Quaternion.Lerp(elementToEffect.rotation, rotZero, Time.deltaTime * 10f);
            }
        }

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
            canTransition = true;

            if (isLvlUpScreen)
            {
                displayText.text = whatToDisplay;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            canTransition = false;
            
            if (isLvlUpScreen)
            {
                displayText.text = String.Empty;
            }
        }
    }
}