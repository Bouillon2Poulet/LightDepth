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

    public void newAction(DrawingToolManager.ToolType toolType)
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
        actionHistory.First().transform.SetParent(actionsList.transform);
        actionHistory.First().GetComponent<Action>().type = toolType;
        actionHistory.First().name = toolType.ToString();
        actionHistory.First().GetComponent<Text>().text = toolType.ToString();
    }

    public void UNDOAction()
    {
        int index = 0;
        foreach (Action.ActionData action in actionHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().actionDatas)
        {
            DrawingManager.GetComponent<DrawingManager>().setDrawingTexturePixel(action.position, action.previousColor);
            index++;
        }
        DrawingManager.GetComponent<DrawingManager>().applyDrawingTexture();
        currentActionIndexFromTopOfStack++;
    }
    public void REDOAction()
    {
        int index = 0;
        currentActionIndexFromTopOfStack--;
        foreach (Action.ActionData action in actionHistory.ElementAt(currentActionIndexFromTopOfStack).GetComponent<Action>().actionDatas)
        {
            DrawingManager.GetComponent<DrawingManager>().setDrawingTexturePixel(action.position, action.newColor);
            index++;
        }
        DrawingManager.GetComponent<DrawingManager>().applyDrawingTexture();
    }

}
