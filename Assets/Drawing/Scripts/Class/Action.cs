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
    public List<ActionDatas> actionDatas;

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
        if (tool == ToolsManager.Tool.eraser) return "pen";
        if (tool == ToolsManager.Tool.colorpicker) return "clrpick";
        else return "error";
    }
    public void addActionDatas(ActionDatas actionDatas)
    {
        this.actionDatas.Add(actionDatas);
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
}

