using UnityEngine;
using UnityEngine.EventSystems;

public class tool_button : MonoBehaviour, IPointerClickHandler
{
    public GameObject tool_icon;
    public Vector2 iconInitialPosition;
    public Vector2 backgroundInitialPosition;

    public ToolsManager.Tool toolType;
    public ToolsManager toolsManager;
    public void Start()
    {
        iconInitialPosition = tool_icon.GetComponent<RectTransform>().anchoredPosition;
        backgroundInitialPosition = GetComponent<RectTransform>().anchoredPosition;
        string nameParsed = name.Split("_")[1];
        toolsManager = GetComponentInParent<ToolsManager>();
        ToolsManager.Tool nameAsTool = toolsManager.textToTool(nameParsed);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            toolsManager.setCurrentTool(toolType);
        }
        setToolButtonActive();
    }

    public void setToolButtonActive()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = toolsManager.activeBackgroundSprite;
        GetComponent<RectTransform>().anchoredPosition += new Vector2(1.3f, 0);
        tool_icon.GetComponent<RectTransform>().anchoredPosition += new Vector2(1, -1);

    }

    public void resetToolButton()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = toolsManager.passiveBackgroundSprite;
        GetComponent<RectTransform>().anchoredPosition = backgroundInitialPosition;
        tool_icon.GetComponent<RectTransform>().anchoredPosition = iconInitialPosition;
    }
}
