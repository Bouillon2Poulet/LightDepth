using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayersManager : MonoBehaviour
{
    public enum LayerType
    {
        Background,
        ColorMap,
        HeightMap
    }

    public GameObject drawingMesh;

    public List<Layer> layers;
    public static Vector2Int texturesSize = new Vector2Int(100, 100);
    public static string pathToTempTexturesFolder = System.IO.Path.Combine(Application.dataPath + "/Drawing/DrawingMesh/TexturesTemp/");
    int currentLayerIndex = 0;

    public ModeManager modeManager;

    public Layer Background;
    public Layer ColorMap;
    public Layer HeightMap;

    public void initLayers(Vector2Int inputTexturesSize, string inputPathToTempTexturesFolder)
    {
        texturesSize = inputTexturesSize;
        pathToTempTexturesFolder = inputPathToTempTexturesFolder;

        layers = new List<Layer>{
            Background,
            ColorMap,
            HeightMap
        };

        setCurrentLayerIndex(1);
    }

    public Texture2D getCurrentLayerTexture()
    {
        if (currentLayerIndex >= 0 && currentLayerIndex < layers.Count)
        {
            return layers.ElementAt(currentLayerIndex).texture;
        }
        else
        {
            // Gérer l'erreur, par exemple en retournant une texture vide ou en lançant une exception.
            Debug.LogError("Invalid layer index");
            return layers[0].texture; // Retourne la première texture par défaut, à changer selon vos besoins.
        }
    }

    public static void fillTextureWithColor(Texture2D texture, Color color)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
    }

    public void setCurrentLayerIndex(int index)
    {
        //TODO : RENDRE PLUS CLAIR LE PASSAGE ENTRE LES MODES

        currentLayerIndex = index;
        foreach (Layer layer in layers)
        {
            layer.setDefaultBackgroundColor();
        }
        layers.ElementAt(currentLayerIndex).setSelectedBackgroundColor();

        if (currentLayerIndex == 2 && modeManager.currentMode == ModeManager.Mode.color) //HeightMap
        {
            modeManager.swapCurrentMode();
            Background.setVisible(false);
            HeightMap.setVisible(true);
        }

        else if (currentLayerIndex == 1 && modeManager.currentMode == ModeManager.Mode.height) //HeightMap
        {
            modeManager.swapCurrentMode();
            Background.setVisible(true);
            HeightMap.setVisible(false);
        }

        else if (modeManager.currentMode == ModeManager.Mode.height)
        {
            modeManager.currentMode = ModeManager.Mode.color;
            HeightMap.setVisible(false);
        }
    }
}
