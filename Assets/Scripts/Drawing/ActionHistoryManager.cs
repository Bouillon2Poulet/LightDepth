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

    public GameObject actionPrefab;
    public Stack<GameObject> actionsHistory;

    public GameObject actionsList;

    public int currentActionIndexFromTopOfStack = 0; //0 = top of the stack

    // Start is called before the first frame update
    void Start()
    {
        actionsHistory = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void newAction(DrawingToolManager.ToolType toolType)
    {
        if (GetComponentInParent<DrawingManagerOld>().mode == DrawingManagerOld.DrawingMode.color)

            if (currentActionIndexFromTopOfStack > 0)
            {
                for (; currentActionIndexFromTopOfStack > 0; currentActionIndexFromTopOfStack--)
                {
                    Destroy(actionsHistory.First());
                    actionsHistory.Pop();
                }
            }
        actionsHistory.Push(Instantiate(actionPrefab));
        actionsHistory.First().transform.SetParent(actionsList.transform);
        // actionsHistory.First().GetComponent<Action>().type = toolType;
        actionsHistory.First().name = toolType.ToString();
        actionsHistory.First().GetComponent<Text>().text = toolType.ToString();
        updateActionsHistoryColor();
    }

    public void UNDOAction()
    {
        int index = 0;
        // foreach (Action.ActionData action in actionsHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().actionDatas)
        // {
        //     GetComponentInParent<DrawingManagerOld>().setDrawingTexturePixel(action.position, action.previousColor);
        //     index++;
        // }
        GetComponentInParent<DrawingManagerOld>().applyDrawingTexture();
        currentActionIndexFromTopOfStack++;
        updateActionsHistoryColor();
    }
    public void REDOAction()
    {
        int index = 0;
        currentActionIndexFromTopOfStack--;
        // foreach (ActionClass.ActionData action in actionsHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().actionDatas)
        // {
        //     GetComponentInParent<DrawingManagerOld>().setDrawingTexturePixel(action.position, action.newColor);
        //     index++;
        // }
        GetComponentInParent<DrawingManagerOld>().applyDrawingTexture();
        updateActionsHistoryColor();
    }

    public void updateActionsHistoryColor()
    {
        int index = 0;
        foreach (GameObject action in actionsHistory)
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
}
