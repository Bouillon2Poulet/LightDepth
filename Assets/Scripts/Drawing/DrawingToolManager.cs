using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingToolManager : MonoBehaviour
{
    public enum ToolType
    {
        pen,
        eraser,
        bucket,
        color_picker,
        size
    }

    public GameObject colorManager;

    private ToolType selectedTool;


    public void setCurrentToolTypeFromString(string toolName)
    {
        selectedTool = System.Enum.Parse<ToolType>(toolName);
        Debug.Log(selectedTool);
    }

    public void performSelectedToolAction(Texture2D drawingTexture, List<Vector2> pixelsCoordinates, Action currentAction)
    {
        switch (selectedTool)
        {
            case ToolType.pen:
                draw(drawingTexture, pixelsCoordinates, currentAction);
                break;
            case ToolType.eraser:
                erase(drawingTexture, pixelsCoordinates, currentAction);
                break;
            case ToolType.bucket:
                // fill(drawingTexture, pixelsCoordinates.Last(), currentColor);
                break;
            case ToolType.color_picker:
                pickColor(drawingTexture, pixelsCoordinates.Last());
                break;
            case ToolType.size: return;
        }
    }
    public void draw(Texture2D drawingTexture, List<Vector2> pixelsCoordinates, Action currentAction)
    {
        currentAction.addActionData(pixelsCoordinates, drawingTexture, colorManager.GetComponent<ColorManager>().getCurrentColor());
        foreach (Vector2 pixelCoordinates in pixelsCoordinates)
        {
            drawingTexture.SetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y, colorManager.GetComponent<ColorManager>().getCurrentColor());
        }
    }

    void erase(Texture2D drawingTexture, List<Vector2> pixelsCoordinates, Action currentAction)
    {
        Color eraseColor = new Color(1, 1, 1, 0);
        currentAction.addActionData(pixelsCoordinates, drawingTexture, eraseColor);
        foreach (Vector2 pixelCoordinates in pixelsCoordinates)
        {
            drawingTexture.SetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y, eraseColor);
        }
    }

    void fill(Texture2D drawingTexture, Vector2 pixelCoordinates)
    {

    }

    void pickColor(Texture2D drawingTexture, Vector2 pixelCoordinates)
    {
        Color textureColor = drawingTexture.GetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y);
        textureColor.a = 1;
        colorManager.GetComponent<ColorManager>().setCurrentColor(textureColor);
    }

    public bool selectedToolCreatesAction()
    {
        if (selectedTool == DrawingToolManager.ToolType.pen || selectedTool == ToolType.eraser)
        {
            return true;
        }
        return false;
    }

    public ToolType getSelectedToolType()
    {
        return selectedTool;
    }
}
