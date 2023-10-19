using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawingToolManager : MonoBehaviour
{
    public enum toolType
    {
        pen,
        eraser,
        bucket,
        color_picker,
        size
    }

    private toolType currentToolType;

    public GameObject toolButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // for (int i = 0; i < (int)toolType.size; i++)
        // {
        //     drawingToolsButton[i] = Instantiate(toolButtonPrefab);
        //     // drawingToolsButton[i].GetComponent<ToolButtonManager>().typeOfTool = indexToToolType(i);
        // }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // public void setCurrentToolType(toolType type)
    // {
    //     currentToolType = type;
    // }

    public void setCurrentToolTypeFromString(string toolName)
    {
        currentToolType = System.Enum.Parse<toolType>(toolName);
        Debug.Log(currentToolType);
    }

    // public void stringToToolType(toolType type)
    // {
    //     currentToolType = type;
    // }

    public string toolTypeToString(toolType toolType)
    {
        switch ((int)toolType)
        {
            case 0:
                return "pen";
            case 1:
                return "eraser";
            case 2:
                return "bucket";
            case 3:
                return "color_picker";
            default: return "size";
        }
    }

    public toolType indexToToolType(int index)
    {
        switch (index)
        {
            case 0:
                return toolType.pen;
            case 1:
                return toolType.eraser;
            case 2:
                return toolType.bucket;
            case 3:
                return toolType.color_picker;
            default: return toolType.size;
        }
    }

    public void action(Texture2D drawingTexture, Vector2Int pixelCoordinates)
    {
        switch (currentToolType)
        {
            // case toolType.pen:
            //     draw(drawingTexture, pixelCoordinates);
            //     break;
            case toolType.eraser:
                erase(drawingTexture, pixelCoordinates);
                break;
            case toolType.bucket:
                fill(drawingTexture, pixelCoordinates);
                break;
            case toolType.color_picker:
                pickColor(drawingTexture, pixelCoordinates);
                break;
            case toolType.size: return;
        }
    }
    public void draw(Texture2D drawingTexture, List<Vector2> pixelsCoordinates)
    {
        foreach (Vector2 pixelCoordinates in pixelsCoordinates)
        {
            drawingTexture.SetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y, Color.black);

        }
    }

    void erase(Texture2D drawingTexture, Vector2Int pixelCoordinates)
    {
        drawingTexture.SetPixel(pixelCoordinates.x, pixelCoordinates.y, new Color(1, 1, 1, 0));
    }

    void fill(Texture2D drawingTexture, Vector2Int pixelCoordinates)
    {

    }

    void pickColor(Texture2D drawingTexture, Vector2Int pixelCoordinates)
    {

    }
}
