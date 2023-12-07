using UnityEngine;
using HSVPicker;
public static class ToolColorPicker
{
    public static void pick(Texture2D texture, Vector2Int pickingPosition, ColorPicker colorPicker)
    {
        Color pickedColor = texture.GetPixel(pickingPosition.x, pickingPosition.y);
        if (pickedColor.a != 1) pickedColor.a = 1;
        colorPicker.CurrentColor = pickedColor;
    }
}
