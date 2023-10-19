using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    public bool isNext;
    bool canBeClicked;
    public void Update()
    {
        canBeClicked = isNext && GetComponentInParent<ActionHistoryManager>().currentActionIndexFromTopOfStack > 0 ? true : !isNext && GetComponentInParent<ActionHistoryManager>().currentActionIndexFromTopOfStack < GetComponentInParent<ActionHistoryManager>().actionHistory.Count ? true : false;
        if (!canBeClicked)
        {
            GetComponent<UnityEngine.UI.Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().color = Color.white    ;
        }
    }
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
        if (pointerEventData.button == PointerEventData.InputButton.Left && canBeClicked)
        {
            if (isNext)
            {
                GetComponentInParent<ActionHistoryManager>().REDOAction();
            }
            else
            {
                GetComponentInParent<ActionHistoryManager>().UNDOAction();
            }
        }
    }
}