using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButtonOld : MonoBehaviour, IPointerClickHandler
{
    public bool isNext;
    bool canBeClicked;
    public void Update()
    {
        canBeClicked = isNext && GetComponentInParent<ActionHistoryManager>().currentActionIndexFromTopOfStack > 0 ? true : !isNext && GetComponentInParent<ActionHistoryManager>().currentActionIndexFromTopOfStack < GetComponentInParent<ActionHistoryManager>().actionsHistory.Count ? true : false;
        if (!canBeClicked)
        {
            GetComponent<UnityEngine.UI.Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
    }
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
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