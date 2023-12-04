using UnityEngine;

public static class ToolPen
{
    public static void draw(Texture2D texture, CursorDatas cursorDatas, Action action, Color currentColor)
    {
        foreach (Vector2Int pixel in cursorDatas.overedPositionsBetweenTwoFrames)
        {
            texture.SetPixel(pixel.x, pixel.y, currentColor);
        }
        texture.Apply();
    }
}
