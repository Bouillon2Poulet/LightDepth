using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorWheel : MonoBehaviour, IPointerClickHandler
{
    public Texture2D colorWheelTexture;
    private Vector2 positionOnColorWheelTexture;
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (GetComponent<UnityEngine.UI.Image>().enabled)

            //Use this to tell when the user left-clicks on the Button
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                positionOnColorWheelTexture = pointerEventData.position - new Vector2(transform.position.x, transform.position.y) + new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height) / 2f;
                GetComponentInParent<ColorManager>().setCurrentColor(colorWheelTexture.GetPixel((int)positionOnColorWheelTexture.x, (int)positionOnColorWheelTexture.y));
            }
    }
}