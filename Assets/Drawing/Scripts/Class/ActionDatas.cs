using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDatas : ScriptableObject
{
    List<Vector2Int> positions;
    List<Color> colorsBeforeAction;
    List<Color> colorsAfterAction;

    // public void addData(Vector2Int position, Color colorBeforeAction, Color colorAfterAction)
    // {
    //     positions.Add(position);
    //     colorsBeforeAction.Add(colorAfterAction);
    //     colorsBeforeAction.Add(colorBeforeAction);
    // }

    // public void printDatas()
    // {
    //     int i = 0;
    //     foreach (Vector2Int position in positions)
    //     {
    //         Debug.Log(position.x + "/" + position.y + "__clr before :" + colorsBeforeAction[i] + "/_clr after :" + colorsAfterAction[i]);
    //         i++;
    //     }
    // }
}
