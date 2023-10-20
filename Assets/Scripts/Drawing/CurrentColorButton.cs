using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentColorButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject colorWheel;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("colorClicked");
            colorWheel.GetComponent<UnityEngine.UI.Image>().enabled = !colorWheel.GetComponent<UnityEngine.UI.Image>().enabled;
        }
    }
    public void updateColorButtonColor()
    {
        GetComponent<UnityEngine.UI.Image>().color = GetComponentInParent<ColorManager>().getCurrentColor();
    }
}