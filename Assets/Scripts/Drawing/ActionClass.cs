using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Action : MonoBehaviour
{
    public ActionHistoryManager.ActionType type;
    public List<Vector2> PixelsList;
    public List<Color> PreviousColor;
    public List<Color> NewColor;

    // public Action()
    // {
    //     PixelsList = new List<Vector2>();
    //     PreviousColor = new List<Color>();
    //     NewColor = new List<Color>();
    // }
    public void addActionData(ActionHistoryManager.ActionType actionType, List<Vector2> pixelsLists, Texture2D texture, Color newColor)
    {
        type = actionType;
        name = actionType.ToString();
        GetComponent<Text>().text = name;
        foreach (Vector2 pixel in pixelsLists)
        {
            if (PixelsList.Count() == 0 || !PixelsList.Contains(pixel))
            {
                PixelsList.Add(pixel);
                PreviousColor.Add(texture.GetPixel((int)PixelsList.Last().x, (int)PixelsList.Last().y));
                NewColor.Add(newColor);
            }
        }
    }

    public void printActionDatas()
    {
        int i = 0;
        foreach (Vector2 pixel in PixelsList)
        {
            // Debug.Log(i);
            Debug.Log("\nPixel:" + pixel.x + "/" + pixel.y);
            Debug.Log("\nPreviousColor: " + PreviousColor[i].ToString() + "// NewColor: " + NewColor[i].ToString());
            i++;
        }
    }

    public void eraseLastPixelDataIfSameAsNew(Vector2 firstPixelOfList)
    {
        if (PixelsList.Count > 0)
            if (PixelsList.Count > 0 && PixelsList.Last() == firstPixelOfList)
            {
                PixelsList.RemoveAt(PixelsList.Count());
                PreviousColor.RemoveAt(PixelsList.Count());
                NewColor.RemoveAt(PixelsList.Count());
            }
    }
}
