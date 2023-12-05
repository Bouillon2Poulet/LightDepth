using System.Collections.Generic;
using UnityEngine;

public static class ToolEraser
{
    static Color eraseColor = new Color(1, 1, 1, 0);
    public static void erase(Texture2D texture, CursorDatas cursorDatas, Action action)
    {
        List<Action.ActionData> actionDatas = new List<Action.ActionData>();
        foreach (Vector2Int pixel in cursorDatas.overedPositionsBetweenTwoFrames)
        {
            Action.ActionData actionData = new Action.ActionData();
            actionData.position = pixel;
            actionData.colorBeforeAction = texture.GetPixel(pixel.x, pixel.y);
            actionData.colorAfterAction = eraseColor;
            actionDatas.Add(actionData);

            texture.SetPixel(pixel.x, pixel.y, eraseColor);
        }
        action.addActionDatas(actionDatas);
        texture.Apply();
    }
}
