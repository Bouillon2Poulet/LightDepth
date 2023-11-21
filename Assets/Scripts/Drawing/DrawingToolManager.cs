using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingToolManager : MonoBehaviour
{
    public GameObject DrawingMeshCamera;
    public enum ToolType
    {
        pen,
        eraser,
        bucket,
        color_picker,
        zoom,
        size
    }

    public bool pointerIsOnUI;
    public GameObject colorManager;
    public GameObject ToolCursor;


    public ToolType selectedTool;

    public void Start()
    {
        selectedTool = ToolType.pen;
    }
    public void setCurrentToolTypeFromString(string toolName, Sprite sprite)
    {
        selectedTool = System.Enum.Parse<ToolType>(toolName);
        ToolCursor.GetComponent<ToolCursor>().setToolCursorTexture(sprite);
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
                // currentAction.printActionDatas();
                //Faire qu'une fois et pas à chaque frame
                if (currentAction.actionDatas.Count == 0)
                {
                    Debug.Log("Appel depuis le switch");
                    Color colorToFill = drawingTexture.GetPixel((int)pixelsCoordinates.Last().x, (int)pixelsCoordinates.Last().y);
                    fill(colorToFill, pixelsCoordinates.Last(), drawingTexture, currentAction, 0);
                    Debug.Log("Fin switch");
                }
                break;
            case ToolType.color_picker:
                pickColor(drawingTexture, pixelsCoordinates.Last());
                break;
            case ToolType.zoom:
                DrawingMeshCamera.GetComponent<CameraMovement>().zoom();
                break;
            case ToolType.size: return;
        }
    }
    public void draw(Texture2D drawingTexture, List<Vector2> pixelsCoordinates, Action currentAction)
    {
        Debug.Log("!");
        // currentAction.addActionData(pixelsCoordinates, drawingTexture, colorManager.GetComponent<ColorManager>().getCurrentColor());
        Debug.Log("?");

        foreach (Vector2 pixelCoordinates in pixelsCoordinates)
        {
            drawingTexture.SetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y, colorManager.GetComponent<ColorManager>().getCurrentColor());
        }
    }

    void erase(Texture2D drawingTexture, List<Vector2> pixelsCoordinates, Action currentAction)
    {
        Color eraseColor = new Color(1, 1, 1, 0);
        // currentAction.addActionData(pixelsCoordinates, drawingTexture, eraseColor);
        foreach (Vector2 pixelCoordinates in pixelsCoordinates)
        {
            drawingTexture.SetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y, eraseColor);
        }
    }


    enum caseType
    {
        zero,
        left,
        right,
        top,
        bot
    }
    void fill(Color colorToFill, Vector2 positionToFill, Texture2D drawingTexture, Action currentAction, caseType caseWhoCalledFunction)
    {
        if (positionToFill.x < 0 || positionToFill.x >= drawingTexture.width ||
            positionToFill.y < 0 || positionToFill.y >= drawingTexture.height)
        {
            // Arrêt si nous sommes en dehors des limites de la texture.
            return;
        }

        if (drawingTexture.GetPixel((int)positionToFill.x, (int)positionToFill.y) != colorToFill)
        {
            // Arrêt si la couleur du pixel ne correspond pas à la couleur à remplir.
            return;
        }

        // if (currentAction.actionDatas.Count != 0)
        // {
        //     if (currentAction.pixelPositionExistActionDatas(positionToFill))
        //     {
        //         // Arrêt si le pixel a déjà été colorié
        //         Debug.Log("Retour !!!!!!!!" + currentAction.actionDatas.Count);
        //         return;
        //     }
        // }

        //TODO faire une fonction qui rajoute un pixel à l'action sans passer par une liste
        List<Vector2> listTemp = new List<Vector2>
        {
            positionToFill
        };
        // currentAction.addActionData(listTemp, drawingTexture, colorManager.GetComponent<ColorManager>().getCurrentColor());
        drawingTexture.SetPixel((int)positionToFill.x, (int)positionToFill.y, colorManager.GetComponent<ColorManager>().getCurrentColor());

        Debug.Log("Taille actionDatas avant voisin" + currentAction.actionDatas.Count);

        // Appels récursifs pour les voisins
        if (caseWhoCalledFunction != caseType.right) fill(colorToFill, new Vector2(positionToFill.x - 1, positionToFill.y), drawingTexture, currentAction, caseType.left); // Gauche
        if (caseWhoCalledFunction != caseType.left) fill(colorToFill, new Vector2(positionToFill.x + 1, positionToFill.y), drawingTexture, currentAction, caseType.right); // Droite
        if (caseWhoCalledFunction != caseType.top) fill(colorToFill, new Vector2(positionToFill.x, positionToFill.y + 1), drawingTexture, currentAction, caseType.bot); // Haut
        if (caseWhoCalledFunction != caseType.top) fill(colorToFill, new Vector2(positionToFill.x, positionToFill.y - 1), drawingTexture, currentAction, caseType.top); // Bas
    }

    void pickColor(Texture2D drawingTexture, Vector2 pixelCoordinates)
    {
        Color textureColor = drawingTexture.GetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y);
        textureColor.a = 1;
        colorManager.GetComponent<ColorManager>().setCurrentColor(textureColor);
    }

    public bool selectedToolCreatesAction()
    {
        if (selectedTool == ToolType.color_picker || selectedTool == ToolType.zoom)
        {
            return false;
        }
        return true;
    }

    public ToolType getSelectedToolType()
    {
        return selectedTool;
    }
}
