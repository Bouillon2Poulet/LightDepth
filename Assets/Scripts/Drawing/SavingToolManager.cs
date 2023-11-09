using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SavingToolManager : MonoBehaviour
{
    public void save()
    {
        Debug.Log("Saving");
        Texture2D drawingTexture = GetComponentInParent<DrawingManager>().getDrawingTexture();
        float minimumX = drawingTexture.width;
        float maximumX = 0;
        float minimumY = drawingTexture.height;
        float maximumY = 0;

        //TODO : find a more optimized way + separate in a function
        for (int y = 0; y < drawingTexture.height; y++)
        {
            for (int x = 0; x < drawingTexture.width; x++)
            {
                if (drawingTexture.GetPixel(x, y).a > 0)
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

        Texture2D finalTexture = new Texture2D((int)calculatedTextureSize.x, (int)calculatedTextureSize.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = "finalTexture",
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };

        Debug.Log("finalTexture" + finalTexture.width + "," + finalTexture.height);

        //TODO : separate in a function
        for (int y = 0; y < finalTexture.height; y++)
        {
            for (int x = 0; x < finalTexture.width; x++)
            {
                finalTexture.SetPixel(x, y, drawingTexture.GetPixel((int)(minimumX + x), (int)(minimumY + y))); ;
            }
        }
        finalTexture.Apply();

        //Saving
        var filePath = System.IO.Path.Combine(Application.dataPath + "/drawingTexture/", finalTexture.name + ".png");
        System.IO.File.WriteAllBytes(filePath, finalTexture.EncodeToPNG());
    }

}
