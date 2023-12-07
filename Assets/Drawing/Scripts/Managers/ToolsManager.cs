using System.Collections;
using System.Collections.Generic;
using HSVPicker;
using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    public enum Tool
    {
        pen,
        eraser,
        bucket,
        colorpicker,
        zoom,
        hand
    }

    public Tool currentTool = Tool.pen;

    public List<GameObject> tools;
    public GameObject tools_pen;
    public GameObject tools_eraser;
    public GameObject tools_bucket;
    public GameObject tools_colorpicker;
    public GameObject tools_zoom;
    public GameObject tools_hand;

    public Camera MeshCamera;
    public ModeManager modeManager;
    public Sprite passiveBackgroundSprite;
    public Sprite activeBackgroundSprite;

    // Start is called before the first frame update
    void Start()
    {
        tools = new List<GameObject>{
            tools_pen,
            tools_eraser,
            tools_bucket,
            tools_colorpicker,
            tools_zoom,
            tools_hand
        };
        tools[0].GetComponent<tool_button>().setToolButtonActive();
    }

    public void setCurrentTool(Tool tool)
    {
        currentTool = tool;
        foreach (GameObject toolIterator in tools)
        {
            toolIterator.GetComponent<tool_button>().resetToolButton();
        }
    }

    public Tool textToTool(string text)
    {
        if (text == "pen") return Tool.pen;
        if (text == "eraser") return Tool.eraser;
        if (text == "bucket") return Tool.bucket;
        if (text == "colorpicker") return Tool.colorpicker;
        if (text == "zoom") return Tool.zoom;
        if (text == "hand") return Tool.hand;
        else
        {
            return Tool.pen;
        }
    }

    public void useTool(Texture2D currentTexture, CursorDatas cursorDatas, Action currentAction, ColorPaletteManager colorPaletteManager)
    {
        switch (currentTool)
        {
            case Tool.pen:
                ToolPen.draw(currentTexture, cursorDatas, currentAction, colorPaletteManager.CurrentColor);
                break;
            case Tool.eraser:
                ToolEraser.erase(currentTexture, cursorDatas, currentAction);
                break;
            case Tool.bucket:
                ToolBucket.fill(currentTexture, cursorDatas.currentPixelPosition, currentAction, currentTexture.GetPixel(cursorDatas.currentPixelPosition.x, cursorDatas.currentPixelPosition.y), colorPaletteManager.CurrentColor, ToolBucket.CaseType.zero);
                currentTexture.Apply();
                break;
            case Tool.colorpicker:
                ToolColorPicker.pick(currentTexture, cursorDatas.currentPixelPosition, modeManager.getCurrentColorPicker());
                break;
            case Tool.zoom:
                ToolCam.zoom(cursorDatas.lastClickedPosition, ref MeshCamera);
                break;
            case Tool.hand:
                ToolCam.pan(cursorDatas.lastClickedPosition, ref MeshCamera);
                break;
        }
    }
}
