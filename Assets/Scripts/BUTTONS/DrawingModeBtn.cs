using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingModeBtn : MonoBehaviour, IPointerClickHandler
{
    private Color activeColor = Color.black;
    private Color disabledColor = Color.grey;
    public bool isColorBtn;
    //Detect if a click occurs
    private void Start()
    {
        GetComponent<UnityEngine.UI.Image>().color = isColorBtn ? activeColor : disabledColor;
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            // GetComponentInParent<DrawingManager>().switchDrawingMode();
        }
    }
    public void switchColor()
    {
        GetComponent<UnityEngine.UI.Image>().color = GetComponent<UnityEngine.UI.Image>().color == activeColor ? disabledColor : activeColor;
    }
}