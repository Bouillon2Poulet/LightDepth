using UnityEngine;
using UnityEngine.EventSystems;

public class AddNewColorToPaletteButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color DefaultColor;
    public Color OveredColor;
    public UnityEngine.UI.Image BackgroundImage;

    public void Start()
    {
        BackgroundImage.color = DefaultColor;
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            GetComponentInParent<ColorPaletteManager>().addNewColorToPalette();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        BackgroundImage.color = OveredColor;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        BackgroundImage.color = DefaultColor;
    }
}
