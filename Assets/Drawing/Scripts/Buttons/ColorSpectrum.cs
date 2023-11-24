using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;

public class ColorSpectrum : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D spectrumTexture;
    public GameObject color_spectrum_cursor;
    private Vector2 positionOnTexture;
    private bool areaEntered = false;
    private bool needForANewClick = true;
    private bool canSetColor = false;
    //Detect if a click occurs
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        areaEntered = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        areaEntered = false;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log("cliqué");
        if (areaEntered)
        {
            needForANewClick = false;
            canSetColor = true;
        }
        else needForANewClick = true;
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            needForANewClick = true;
            canSetColor = false;
        }
        if (canSetColor && !needForANewClick)
        {
            if (Input.GetMouseButton(0))
            {
                positionOnTexture = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - new Vector2(transform.position.x, transform.position.y) + new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height) * 3 / 2) / 3;
                positionOnTexture = new Vector2(Mathf.Clamp(positionOnTexture.x, 0, 85), Mathf.Clamp(positionOnTexture.y, 0, 85));
                SetCursorPosition(positionOnTexture);
                Color clickedColor = spectrumTexture.GetPixel((int)positionOnTexture.x, (int)positionOnTexture.y);
                ColorPaletteButton.ColorAndCoordinates colorAndCoordinates = new ColorPaletteButton.ColorAndCoordinates { color = clickedColor, coordinates = positionOnTexture };
                GetComponentInParent<ColorsManager>().ModifyColor(colorAndCoordinates);
            }
        }
    }

    public List<ColorPaletteButton.ColorAndCoordinates> initialPaletteColorsAndCoordinates(int paletteSize)
    {
        List<ColorPaletteButton.ColorAndCoordinates> colorAndCoordinatesList = new List<ColorPaletteButton.ColorAndCoordinates>
        {
            new ColorPaletteButton.ColorAndCoordinates { color = Color.black, coordinates = new Vector2(0, 0) },
            new ColorPaletteButton.ColorAndCoordinates { color = Color.white, coordinates = new Vector2(spectrumTexture.width, spectrumTexture.height)}
        };

        if (!name.Contains("height"))
        {
            float offset = 6;
            for (int i = 0; i < paletteSize - 2; i++)
            {
                Vector2 coordinates = new Vector2(i * offset, spectrumTexture.height / 2);
                colorAndCoordinatesList.Add(new ColorPaletteButton.ColorAndCoordinates { color = spectrumTexture.GetPixel((int)coordinates.x, (int)coordinates.y), coordinates = coordinates });
            }
        }
        else
        {
            float offset = 7;
            for (int i = 0; i < paletteSize - 2; i++)
            {
                Vector2 coordinates = new Vector2(spectrumTexture.width / 2, spectrumTexture.height - i * offset);
                colorAndCoordinatesList.Add(new ColorPaletteButton.ColorAndCoordinates { color = spectrumTexture.GetPixel((int)coordinates.x, (int)coordinates.y), coordinates = coordinates });
            }
        }
        return colorAndCoordinatesList;
    }

    public void SetCursorPosition(Vector2 coordinates)
    {
        color_spectrum_cursor.GetComponent<RectTransform>().anchoredPosition = coordinates - new Vector2(spectrumTexture.width / 2, spectrumTexture.height / 2);
    }

    public void InitSpectrumTexture()
    {
        Vector2Int textureSize = new Vector2Int(85, 85);

        Texture2D spectrumTexture = new Texture2D(textureSize.x, textureSize.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = "spectrumTexture",
            filterMode = FilterMode.Bilinear,
            alphaIsTransparency = true
        };

        for (int y = 0; y < textureSize.y; y++)
        {
            // Calcul de la luminosité en fonction de la position verticale
            float luminosity = Mathf.Clamp01(2f * (float)y / textureSize.y);
            float saturation = (float)y / textureSize.y < 0.5f ? 1f : (1f - (float)y / textureSize.y) * 2f;

            for (int x = 0; x < textureSize.x; x++)
            {
                // Calcul de la teinte en fonction de la position horizontale (tous les pixels de l'arc-en-ciel)
                float hue = x / (float)textureSize.x;

                // Conversion de HSV à RGB
                Color pixelColor = Color.HSVToRGB(hue, saturation, luminosity);

                spectrumTexture.SetPixel(x, y, pixelColor);
            }
        }

        spectrumTexture.Apply();

        // Appliquer la texture à votre objet, par exemple une image UI
        UnityEngine.UI.Image imageComponent = GetComponent<UnityEngine.UI.Image>();
        if (imageComponent != null)
        {
            imageComponent.sprite = Sprite.Create(spectrumTexture, new Rect(0, 0, spectrumTexture.width, spectrumTexture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("Aucun composant Image trouvé sur l'objet.");
        }
    }

    public void HSLtoPositionInSpectrumTexture(Vector3 HSLcolor)
    {
        
    }

}
