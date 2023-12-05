using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ToolPen
{
    public static void draw(Texture2D texture, CursorDatas cursorDatas, Action action, Color currentColor)
    {
        List<Action.ActionData> actionDatas = new List<Action.ActionData>();
        foreach (Vector2Int pixel in cursorDatas.overedPositionsBetweenTwoFrames)
        {
            Action.ActionData actionData = new Action.ActionData();
            actionData.position = pixel;
            actionData.colorBeforeAction = texture.GetPixel(pixel.x, pixel.y);
            actionData.colorAfterAction = currentColor;
            actionDatas.Add(actionData);

            texture.SetPixel(pixel.x, pixel.y, currentColor);
        }
        action.addActionDatas(actionDatas);
        texture.Apply();
    }
}
