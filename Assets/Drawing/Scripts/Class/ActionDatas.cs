using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDatas : MonoBehaviour
{
    List<Vector2Int> positions;
    List<Color> colorsBeforeAction;
    List<Color> colorsAfterAction;

    public void addData(Vector2Int position, Color colorBeforeAction, Color colorAfterAction)
    {
        positions.Add(position);
        colorsBeforeAction.Add(colorAfterAction);
        colorsBeforeAction.Add(colorBeforeAction);
    }
}
