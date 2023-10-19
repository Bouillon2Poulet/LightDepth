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
                // Debug.Log(new Vector2(pointerEventData.position.x, pointerEventData.position.y));
                // Debug.Log(pointerEventData.position - new Vector2(transform.position.x, transform.position.y) + new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height) / 2f);
                positionOnColorWheelTexture = pointerEventData.position - new Vector2(transform.position.x, transform.position.y) + new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height) / 2f;
                // Debug.Log(new Vector2(pointerEventData.position.x - transform.position.x, pointerEventData.position.y - transform.position.z));
                GetComponentInParent<ColorManager>().currentColor = colorWheelTexture.GetPixel((int)positionOnColorWheelTexture.x, (int)positionOnColorWheelTexture.y);
            }
    }

    // public Vector2 screenPositionToColorWheelPosition(Vector2 screenPosition)
    // {
    //     // return
    // }
    void Start()
    {
    }
    void Update()
    {
    }
}