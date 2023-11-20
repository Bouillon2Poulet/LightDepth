using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class color_palette_spectrum : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D spectrumTexture;
    public GameObject color_spectrum_cursor;
    private Vector2 positionOnTexture;
    private bool areaEntered = false;
    private bool needForANewClick = true;
    //Detect if a click occurs
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        areaEntered = true;
        needForANewClick = needForANewClick == true ? true : false;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (needForANewClick)
        {
            areaEntered = false;
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        needForANewClick = false;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        needForANewClick = true;
    }

    public void Update()
    {
        if (areaEntered && !needForANewClick)
        {
            if (Input.GetMouseButton(0))
            {
                positionOnTexture = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - new Vector2(transform.position.x, transform.position.y) + new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height) * 3 / 2) / 3;
                positionOnTexture = new Vector2(Mathf.Clamp(positionOnTexture.x, 0, 85), Mathf.Clamp(positionOnTexture.y, 0, 85));
                SetCursorPosition(positionOnTexture);
                Color clickedColor = spectrumTexture.GetPixel((int)positionOnTexture.x, (int)positionOnTexture.y);
                color_palette_button.ColorAndCoordinates colorAndCoordinates = new color_palette_button.ColorAndCoordinates { color = clickedColor, coordinates = positionOnTexture };
                GetComponentInParent<colors_manager>().ModifyColor(colorAndCoordinates);
            }
        }
    }

    public List<color_palette_button.ColorAndCoordinates> initialPaletteColorsAndCoordinates(int paletteSize)
    {
        List<color_palette_button.ColorAndCoordinates> colorAndCoordinatesList = new List<color_palette_button.ColorAndCoordinates>
        {
            new color_palette_button.ColorAndCoordinates { color = Color.black, coordinates = new Vector2(0, 0) },
            new color_palette_button.ColorAndCoordinates { color = Color.white, coordinates = new Vector2(spectrumTexture.width, spectrumTexture.height)}
        };

        // float offset = spectrumTexture.width * 2 / paletteSize - 2 - 1;
        if (!name.Contains("height"))
        {
            float offset = 6;
            for (int i = 0; i < paletteSize - 2; i++)
            {
                Vector2 coordinates = new Vector2(i * offset, spectrumTexture.height / 2);
                colorAndCoordinatesList.Add(new color_palette_button.ColorAndCoordinates { color = spectrumTexture.GetPixel((int)coordinates.x, (int)coordinates.y), coordinates = coordinates });
            }
        }
        else
        {
            float offset = 7;
            for (int i = 0; i < paletteSize - 2; i++)
            {
                Vector2 coordinates = new Vector2(spectrumTexture.width / 2, spectrumTexture.height - i * offset);
                colorAndCoordinatesList.Add(new color_palette_button.ColorAndCoordinates { color = spectrumTexture.GetPixel((int)coordinates.x, (int)coordinates.y), coordinates = coordinates });
            }
        }
        return colorAndCoordinatesList;
    }

    public void SetCursorPosition(Vector2 coordinates)
    {
        color_spectrum_cursor.GetComponent<RectTransform>().anchoredPosition = coordinates - new Vector2(spectrumTexture.width / 2, spectrumTexture.height / 2);
    }
}
