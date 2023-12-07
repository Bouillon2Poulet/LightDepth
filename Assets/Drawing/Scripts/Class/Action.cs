using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Action : MonoBehaviour
{
    public ToolsManager.Tool tool;
    public ModeManager.Mode mode;

    public Color colorModeColor;
    public Color heightModeColor;

    public GameObject actionText;
    public GameObject actionModeColor;
    public GameObject actionBackground;

    public Sprite defaultBackgroundColor;
    public Sprite selectedBackgroundColor;

    public int index = 0;


    public struct ActionData
    {
        public Vector2Int position;
        public Color colorBeforeAction;
        public Color colorAfterAction;
    }

    public List<ActionData> actionDatas;

    public void Start()
    {
        actionDatas = new List<ActionData>();
    }
    public void initAction(ToolsManager.Tool tool, ModeManager.Mode mode)
    {
        this.tool = tool;
        this.mode = mode;
        setActionColorAndText();
    }

    public void setActionColorAndText()
    {
        actionModeColor.GetComponent<UnityEngine.UI.Image>().color = mode == ModeManager.Mode.color ? colorModeColor : heightModeColor;
        actionText.GetComponent<TMP_Text>().text = toolAsText(tool);
    }

    public string toolAsText(ToolsManager.Tool tool)
    {
        if (tool == ToolsManager.Tool.pen) return "pen";
        if (tool == ToolsManager.Tool.eraser) return "eraser";
        if (tool == ToolsManager.Tool.bucket) return "fill";
        else return "error";
    }
    public void addActionDatas(List<ActionData> actionDatasToAdd)
    {
        foreach (ActionData actionData in actionDatasToAdd)
        {
            actionDatas.Add(actionData);
        }
    }

    public void resetBackgroundColor()
    {
        actionBackground.GetComponent<UnityEngine.UI.Image>().sprite = defaultBackgroundColor;
    }

    public void setActiveBackgroundColor()
    {
        actionBackground.GetComponent<UnityEngine.UI.Image>().sprite = selectedBackgroundColor;
    }

    public void setIndex(int index)
    {
        this.index = index;
        name = "action" + index;
        actionBackground.name = "actionBackground" + index;
    }

    public void printActionDatas()
    {
        foreach (ActionData data in actionDatas)
        {
            Debug.Log(data.position);
        }
    }

    public bool containsPixel(Vector2Int pixelPosition)
    {
        foreach (ActionData action in actionDatas)
        {
            if (action.position == pixelPosition)
            {
                return true;
            }
        }
        return false;
    }
}

