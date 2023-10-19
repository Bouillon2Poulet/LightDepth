using UnityEngine;
using UnityEngine.EventSystems;

public class ToolButton : MonoBehaviour, IPointerClickHandler
{
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // //Use this to tell when the user right-clicks on the Button
        // if (pointerEventData.button == PointerEventData.InputButton.Right)
        // {
        //     //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        //     Debug.Log(name + " Game Object Right Clicked!");
        // }

        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            string textToRemove = "_btn";
            string nameWithout_btn = this.name.Replace(textToRemove, "");
            GetComponentInParent<DrawingToolManager>().setCurrentToolTypeFromString(nameWithout_btn);
        }
    }
}