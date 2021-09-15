using System;
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
        [SerializeField] private Canvas canvas;
        
        private bool canTransition;
        Vector2 mousePos = Vector2.zero;
        private RectTransform rTrans;
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
             }
            
            //Get the 4 corners (in world space) of the raw image gameobject's rect transform on the GUI
            Vector3[] corners = new Vector3[4];
            rTrans.GetWorldCorners(corners);
            Rect newRect = rTrans.rect;//new Rect(corners[0], corners[2] - corners[0]);
            
            //Get the pixel offset amount from the current mouse position to the left edge of the minimap
            //rect transform.  And likewise for the y offset position.
            float xPositionDeltaPoint = Mouse.current.position.x.ReadValue() - newRect.x;
            float yPositionDeltaPoint = Mouse.current.position.y.ReadValue() - newRect.y;
            
            //Debug.Log("The x position delta is: " + xPositionDeltaPoint);
            //Debug.Log("The y position delta is: " + yPositionDeltaPoint);
            
                
            //The value "170" is the raw image size.
            float compensateForScalingX = elementSize.x * canvas.scaleFactor;
            //"600" is the current reference resolution height on the Canvas Scaler script.
            float compensateForScalingY = elementSize.x * (Screen.height / 600) * canvas.scaleFactor;
            
            //If the game screen height resolution and the canvas scaler script's "y reference resolution" are
            //exactly the same, then the division value will be zero.  Since you can't divide by zero, I need
            //to check for this here.
            if (compensateForScalingY == 0)
            {
                compensateForScalingY = elementSize.x;
            }
            
            //The value "170" is the raw image size currently
            float xPositionCameraCoordinates = (xPositionDeltaPoint / compensateForScalingX);
            float yPositionCameraCoordinates = (yPositionDeltaPoint / compensateForScalingY);
            
            var rotX = new Vector3(10f * xPositionCameraCoordinates, 10f * yPositionCameraCoordinates);
            transform.Rotate(rotX, Space.Self);
        }

        private void FixedUpdate()
        {
            mousePos.Normalize();
            Debug.Log("MousePos is: " + transform.GetComponent<RectTransform>().rect.width);
            Debug.Log("ElementPos is: " + transform.position);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            canTransition = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            canTransition = false;
        }
    }
}