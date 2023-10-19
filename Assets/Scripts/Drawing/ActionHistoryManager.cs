using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionHistoryManager : MonoBehaviour
{
    public enum ActionType
    {
        paint,
        erase,
        size
    }

    public GameObject DrawingManager;
    public GameObject actionPrefab;
    public Stack<GameObject> actionHistory;
    public GameObject actionsList;
    public int currentActionIndexFromTopOfStack = 0; //0 = top of the stack

    // Start is called before the first frame update
    void Start()
    {
        actionHistory = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        foreach (GameObject action in actionHistory)
        {
            if (index < currentActionIndexFromTopOfStack)
            {
                action.GetComponent<Text>().color = Color.grey;
            }
            else
            {
                action.GetComponent<Text>().color = Color.black;
            }
            action.GetComponent<RectTransform>().anchoredPosition = new Vector2(-43, -47 + index * (-50));
            index++;
        }
    }

    public void newAction()
    {
        if (currentActionIndexFromTopOfStack > 0)
        {
            for (; currentActionIndexFromTopOfStack > 0; currentActionIndexFromTopOfStack--)
            {
                Destroy(actionHistory.First());
                actionHistory.Pop();
            }
        }
        actionHistory.Push(Instantiate(actionPrefab));
        actionHistory.First().transform.parent = actionsList.transform;
        // actionHistory.First().GetComponent<RectTransform>().anchoredPosition = new Vector2(-43, -47);
    }

    public void UNDOAction()
    {
        int index = 0;
        foreach (Vector2 pixel in actionHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().PixelsList)
        {
            DrawingManager.GetComponent<DrawingManager>().drawingTexture.SetPixel((int)pixel.x, (int)pixel.y, actionHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().PreviousColor.ElementAt(index));
            index++;
        }
        DrawingManager.GetComponent<DrawingManager>().drawingTexture.Apply();
        currentActionIndexFromTopOfStack++;
    }
    public void REDOAction()
    {
        int index = 0;
        currentActionIndexFromTopOfStack--;
        foreach (Vector2 pixel in actionHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().PixelsList)
        {
            DrawingManager.GetComponent<DrawingManager>().drawingTexture.SetPixel((int)pixel.x, (int)pixel.y, actionHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().NewColor.ElementAt(index));
            index++;
        }
        DrawingManager.GetComponent<DrawingManager>().drawingTexture.Apply();
    }

}
