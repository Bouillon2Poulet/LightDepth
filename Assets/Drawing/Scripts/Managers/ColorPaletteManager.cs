using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HSVPicker;

public class ColorPaletteManager : MonoBehaviour
{
    public UnityEngine.UI.Image CurrentColorImage;
    public Color CurrentColor;
    public List<UnityEngine.UI.Image> ColorPalette;
    public UnityEngine.UI.Image ColorPalette0;
    public UnityEngine.UI.Image ColorPalette1;
    public UnityEngine.UI.Image ColorPalette2;
    public UnityEngine.UI.Image ColorPalette3;
    public UnityEngine.UI.Image ColorPalette4;
    public UnityEngine.UI.Image ColorPalette5;
    public UnityEngine.UI.Image ColorPalette6;
    public UnityEngine.UI.Image ColorPalette7;
    public UnityEngine.UI.Image ColorPalette8;
    public UnityEngine.UI.Image ColorPalette9;
    public UnityEngine.UI.Image ColorPalette10;

    private Color[] ColorPaletteColors;

    public ColorPicker picker;

    // Start is called before the first frame update
    void Start()
    {
        ColorPalette = new List<UnityEngine.UI.Image>
        {
            ColorPalette0,
            ColorPalette1,
            ColorPalette2,
            ColorPalette3,
            ColorPalette4,
            ColorPalette5,
            ColorPalette6,
            ColorPalette7,
            ColorPalette8,
            ColorPalette9,
            ColorPalette10
        };

        ColorPaletteColors = new Color[11];

        initColorPaletteColorsArray();
        updateColorPaletteColorsImages();

        CurrentColor = Color.black;
        picker.CurrentColor = CurrentColor;
        CurrentColorImage.color = CurrentColor;

        picker.onValueChanged.AddListener(color =>
        {
            CurrentColor = color;
            CurrentColorImage.color = CurrentColor;
        });
    }

    private void initColorPaletteColorsArray()
    {
        for (int i = 0; i < 10; i++)
        {
            Color color = HSVUtil.ConvertHsvToRgb(i * 360f / 11, 1, 1, 1);
            ColorPaletteColors[i] = color;
        }
        ColorPaletteColors[10] = Color.white;
    }

    private void updateColorPaletteColorsImages()
    {
        int i = 0;
        foreach (UnityEngine.UI.Image ColorImage in ColorPalette)
        {
            ColorImage.color = ColorPaletteColors[i];
            i++;
        }
    }

    public void addNewColorToPalette()
    {
        for (int i = 10; i > 0; i--)
        {
            ColorPaletteColors[i] = ColorPaletteColors[i - 1];
        }
        ColorPaletteColors[0] = CurrentColorImage.color;
        updateColorPaletteColorsImages();
    }

    public void SetCurrentColorFromArray(int ColorPaletteColorsIndex)
    {
        CurrentColor = ColorPaletteColors[ColorPaletteColorsIndex];
        picker.CurrentColor = CurrentColor;
        CurrentColorImage.color = CurrentColor;
    }
}
