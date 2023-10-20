using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Action : MonoBehaviour
{
    public DrawingToolManager.ToolType type;
    public struct ActionData
    {
        public Vector2 position;
        public Color previousColor;
        public Color newColor;
    }
    public List<ActionData> actionDatas;

    public void Start()
    {
        actionDatas = new List<ActionData>();
    }
    public void addActionData(List<Vector2> pixelsLists, Texture2D texture, Color newColor)
    {
        Debug.Log(pixelsLists[1]);
        foreach (Vector2 pixel in pixelsLists)
        {
            Debug.Log(actionDatas.Count);
            if (actionDatas.Count() == 0 || !pixelPositionExistActionDatas(pixel))
            {
                Debug.Log("OOO");
                ActionData newData;
                newData.position = pixel;
                newData.previousColor = texture.GetPixel((int)pixel.x, (int)pixel.y);
                newData.newColor = newColor;
                actionDatas.Add(newData);
            }
        }
        Debug.Log("!!!");
    }

    public void printActionDatas()
    {
        int i = 0;
        foreach (ActionData actionData in actionDatas)
        {
            Debug.Log(i);
            Debug.Log("\nPixel n°:" + i + " : " + actionData.position.x + "/" + actionData.position.y);
            Debug.Log("\nPreviousColor: " + actionData.previousColor[i].ToString() + "// NewColor: " + actionData.newColor[i].ToString());
            i++;
        }
    }

    public bool pixelPositionExistActionDatas(Vector2 position)
    {
        bool exist = false;
        Debug.Log("AAA");
        if (actionDatas.Count == 0) return true;
        foreach (ActionData actionData in actionDatas)
        {
            if (actionData.position == position)
            {
                exist = true;
            }
        }
        return exist;
    }
}
