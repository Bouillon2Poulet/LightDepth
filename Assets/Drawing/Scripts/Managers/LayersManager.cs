using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class LayersManager : MonoBehaviour
{
    public Texture2D layer1;
    public List<Texture2D> layers;
    private Vector2Int texturesSize;
    private string pathToTempTexturesFolder;
    int currentLayerIndex = 0;

    public void initLayers(Vector2Int inputTexturesSize, string inputPathToTempTexturesFolder)
    {
        texturesSize = inputTexturesSize;
        pathToTempTexturesFolder = inputPathToTempTexturesFolder;
        initTexture(ref layer1, "layer1");
        layers = new List<Texture2D>{
            layer1
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void initTexture(ref Texture2D texture, string name)
    {
        //Init texture
        texture = new Texture2D(texturesSize.x, texturesSize.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = name,
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };
        var filePath = System.IO.Path.Combine(pathToTempTexturesFolder, texture.name + ".png");
        fillTextureWithBlackAndFullTransparency(texture);
        System.IO.File.WriteAllBytes(filePath, texture.EncodeToPNG());
    }

    public Texture2D getCurrentLayerTexture()
    {
        if (currentLayerIndex >= 0 && currentLayerIndex < layers.Count)
        {
            return layers[currentLayerIndex];
        }
        else
        {
            // Gérer l'erreur, par exemple en retournant une texture vide ou en lançant une exception.
            Debug.LogError("Invalid layer index");
            return layers[0]; // Retourne la première texture par défaut, à changer selon vos besoins.
        }
    }

    void fillTextureWithBlackAndFullTransparency(Texture2D texture)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        texture.Apply();
    }
}
