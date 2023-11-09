using UnityEngine;
using UnityEngine.EventSystems;

public class SaveButton : MonoBehaviour, IPointerClickHandler
{
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            GetComponentInParent<SavingToolManager>().save();
        }
    }
}