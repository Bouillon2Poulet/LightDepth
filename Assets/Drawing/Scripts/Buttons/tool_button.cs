using UnityEngine;
using UnityEngine.EventSystems;

public class tool_button : MonoBehaviour, IPointerClickHandler
{
    public GameObject tool_icon;
    public Vector2 iconInitialPosition;
    public Vector2 backgroundInitialPosition;

    public tools_manager.Tool toolType;
    public void Start()
    {
        iconInitialPosition = tool_icon.GetComponent<RectTransform>().anchoredPosition;
        backgroundInitialPosition = GetComponent<RectTransform>().anchoredPosition;
        string nameParsed = name.Split("_")[1];
        tools_manager.Tool nameAsTool = GetComponentInParent<tools_manager>().textToTool(nameParsed);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            GetComponentInParent<tools_manager>().setCurrentTool(toolType);
        }
        setToolButtonActive();
    }

    public void setToolButtonActive()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = GetComponentInParent<tools_manager>().activeBackgroundSprite;
        GetComponent<RectTransform>().anchoredPosition += new Vector2(1.3f, 0);
        tool_icon.GetComponent<RectTransform>().anchoredPosition += new Vector2(1, -1);

    }

    public void resetToolButton()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = GetComponentInParent<tools_manager>().passiveBackgroundSprite;
        GetComponent<RectTransform>().anchoredPosition = backgroundInitialPosition;
        tool_icon.GetComponent<RectTransform>().anchoredPosition = iconInitialPosition;
    }
}
