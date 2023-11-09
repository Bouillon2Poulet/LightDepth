using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.


public class BackgroundTextureGenerator : MonoBehaviour
{
    public int width;
    Texture2D backgroundTexture;

    // Start is called before the first frame update
    void Start()
    {
        // Init texture
        backgroundTexture = new Texture2D(width, width * 1080 / 1980, TextureFormat.RGBA32, false);
        backgroundTexture.name = "backgroundTexture";
        backgroundTexture.filterMode = FilterMode.Point;
        backgroundTexture.alphaIsTransparency = true;

        var filePath = System.IO.Path.Combine(Application.dataPath, "backgroundTexture.png");
        fillBackgroundTextureWithGreyTiles(backgroundTexture);
        System.IO.File.WriteAllBytes(filePath, backgroundTexture.EncodeToPNG());

        GetComponent<Image>().sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2(0.5f, 0.5f));
    }

    void fillBackgroundTextureWithGreyTiles(Texture2D texture)
    {
        bool lightColor = true;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (lightColor)
                {
                    texture.SetPixel(x, y, new Color(0.7f, 0.7f, 0.7f, 1));
                }
                else
                {
                    texture.SetPixel(x, y, new Color(0.8f, 0.8f, 0.8f, 1));
                }
                lightColor = !lightColor;
            }
        }
        texture.Apply();
    }
}
