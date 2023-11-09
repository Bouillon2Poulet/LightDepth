using UnityEngine;
using UnityEngine.EventSystems;

public class ToolButton : MonoBehaviour, IPointerClickHandler
{
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            string textToRemove = "_btn";
            string nameWithout_btn = this.name.Replace(textToRemove, "");
                GetComponentInParent<DrawingToolManager>().setCurrentToolTypeFromString(nameWithout_btn, GetComponent<UnityEngine.UI.Image>().sprite);
        }
    }

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("The cursor entered the selectable UI element.");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("The cursor left the selectable UI element.");
    }
}