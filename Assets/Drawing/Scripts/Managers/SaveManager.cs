using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SaveManager : MonoBehaviour
{
    public string filePath;
    public Texture2D finalColorTexture;
    public Texture2D finalHeightTexture;
    public LayersManager LayersManager;
    public void save()
    {
        Debug.Log("Saving");
        Texture2D colorTexture = LayersManager.ColorMap.texture;
        Texture2D heightTexture = LayersManager.HeightMap.texture;

        float minimumX = colorTexture.width;
        float maximumX = 0;
        float minimumY = colorTexture.height;
        float maximumY = 0;

        //TODO : find a more optimized way + separate in a function
        for (int y = 0; y < colorTexture.height; y++)
        {
            for (int x = 0; x < colorTexture.width; x++)
            {
                if (colorTexture.GetPixel(x, y).a > 0)
                {
                    if (x < minimumX)
                    {
                        minimumX = x;
                    }
                    else if (x > maximumX)
                    {
                        maximumX = x;
                    }
                    if (y < minimumY)
                    {
                        minimumY = y;
                    }
                    else if (y > maximumY)
                    {
                        maximumY = y;
                    }
                }
            }
        }

        Vector2 calculatedTextureSize = new Vector2(maximumX - minimumX + 1, maximumY - minimumY + 1);
        Debug.Log("calculatedTextureSize" + calculatedTextureSize);

        finalColorTexture = new Texture2D((int)calculatedTextureSize.x, (int)calculatedTextureSize.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = "finalColorTexture",
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };

        finalHeightTexture = new Texture2D((int)calculatedTextureSize.x, (int)calculatedTextureSize.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = "finalHeightTexture",
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };


        Debug.Log("finalColorTexture" + finalColorTexture.width + "," + finalColorTexture.height);

        //TODO : separate in a function
        for (int y = 0; y < finalColorTexture.height; y++)
        {
            for (int x = 0; x < finalColorTexture.width; x++)
            {
                finalColorTexture.SetPixel(x, y, colorTexture.GetPixel((int)(minimumX + x), (int)(minimumY + y)));
                finalHeightTexture.SetPixel(x, y, heightTexture.GetPixel((int)(minimumX + x), (int)(minimumY + y)));
            }
        }
        finalColorTexture.Apply();
        finalHeightTexture.Apply();

        //Saving
        filePath = System.IO.Path.Combine(Application.dataPath + "/saveTextures/", finalColorTexture.name + ".png");
        System.IO.File.WriteAllBytes(filePath, finalColorTexture.EncodeToPNG());
        filePath = System.IO.Path.Combine(Application.dataPath + "/saveTextures/", finalHeightTexture.name + ".png");
        System.IO.File.WriteAllBytes(filePath, finalHeightTexture.EncodeToPNG());
    }

}
