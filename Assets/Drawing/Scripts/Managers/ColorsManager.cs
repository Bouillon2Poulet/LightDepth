using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsManager : MonoBehaviour
{
    public GameObject color_palette_1;
    public GameObject color_palette_2;
    public GameObject color_palette_3;
    public GameObject color_palette_4;
    public GameObject color_palette_5;
    public GameObject color_palette_6;
    public GameObject color_palette_7;
    public GameObject color_palette_8;
    public GameObject color_palette_9;
    public GameObject color_palette_10;
    public GameObject color_palette_11;
    public GameObject color_palette_12;
    public GameObject color_palette_13;
    public GameObject color_palette_14;

    public GameObject color_spectrum;

    public List<GameObject> color_palette;

    public Color defaultOutlineColor;
    public Color selectedOutlineColor;

    public int currentPaletteIndex = 0;
    public Color currentColor;

    private bool firstUpdate = true;

    // Start is called before the first frame update
    void Start()
    {
        color_palette = new List<GameObject>{
            color_palette_1,
            color_palette_2,
            color_palette_3,
            color_palette_4,
            color_palette_5,
            color_palette_6,
            color_palette_7,
            color_palette_8,
            color_palette_9,
            color_palette_10,
            color_palette_11,
            color_palette_12,
            color_palette_13,
            color_palette_14
        };

        List<ColorPaletteButton.ColorAndCoordinates> initialColorAndCoordinates = color_spectrum.GetComponent<ColorSpectrum>().initialPaletteColorsAndCoordinates(color_palette.Count);
        int i = 0;
        foreach (GameObject color in color_palette)
        {
            color.GetComponent<ColorPaletteButton>().colorAndCoordinates = initialColorAndCoordinates[i];
            i++;
        }
    }

    void Update()
    {
        if (firstUpdate)
        {
            SetCurrentPaletteIndex(0);
            color_palette[currentPaletteIndex].GetComponent<ColorPaletteButton>().outline.GetComponent<UnityEngine.UI.Image>().color = selectedOutlineColor;
            firstUpdate = false;
        }
    }

    public void SetCurrentPaletteIndex(int index)
    {
        currentPaletteIndex = index;
        foreach (GameObject color_palette_iterator in color_palette)
        {
            color_palette_iterator.GetComponent<ColorPaletteButton>().resetOutlineColor();
        }
        currentColor = color_palette[currentPaletteIndex].GetComponent<ColorPaletteButton>().colorAndCoordinates.color;
        color_spectrum.GetComponent<ColorSpectrum>().SetCursorPosition(color_palette[currentPaletteIndex].GetComponent<ColorPaletteButton>().colorAndCoordinates.coordinates);
    }

    public void ModifyColor(ColorPaletteButton.ColorAndCoordinates colorAndCoordinates)
    {
        color_palette[currentPaletteIndex].GetComponent<ColorPaletteButton>().setColorAndCoordinates(colorAndCoordinates);
        currentColor = color_palette[currentPaletteIndex].GetComponent<ColorPaletteButton>().colorAndCoordinates.color;
    }
}
