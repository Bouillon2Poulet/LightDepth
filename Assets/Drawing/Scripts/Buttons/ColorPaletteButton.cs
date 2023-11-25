using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPaletteButton : MonoBehaviour, IPointerClickHandler
{
    private int indexAsInt;
    public void Start()
    {
        indexAsInt = int.Parse(name);
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            GetComponentInParent<ColorPaletteManager>().SetCurrentColorFromArray(indexAsInt);
        }
    }
}