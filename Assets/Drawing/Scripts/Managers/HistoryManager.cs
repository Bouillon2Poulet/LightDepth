using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    public GameObject actionPrefab;
    public LinkedList<GameObject> history;
    static int currentActionIndex = 0;

    private Vector3 firstActionPosition = new Vector3(687, 112, 0);
    private Vector3 positionOffset = new Vector3(174, 33, 0);

    void Start()
    {
        history = new LinkedList<GameObject>();
    }
    public void newActionIfNecessary(ToolsManager.Tool tool, ModeManager.Mode mode)
    {
        if (toolsCreateAction(tool))
        {
            if (history.Count == 14)
            {
                Destroy(history.Last());
                history.RemoveLast();
            }
            history.AddFirst(Instantiate(actionPrefab));
            history.First().transform.SetParent(transform);
            history.First().GetComponent<Action>().initAction(tool, mode);

            int i = 0;
            foreach (GameObject action in history)
            {
                handleActionPosition(action, i % 7, (int)Mathf.Ceil(i / 7));
                action.GetComponent<Action>().resetBackgroundColor();
                action.GetComponent<Action>().setIndex(i);
                i++;
            }
            history.First().GetComponent<Action>().setActiveBackgroundColor();
        }
    }

    public void handleActionPosition(GameObject action, int row, int column)
    {
        action.transform.position = firstActionPosition + new Vector3(column * positionOffset.x, -row % 7 * positionOffset.y, 0);
    }
    public void updateCurrentAction(ActionDatas actionDatas)
    {
        history.ElementAt(currentActionIndex).GetComponent<Action>().addActionDatas(actionDatas);
    }

    public bool toolsCreateAction(ToolsManager.Tool tool)
    {
        if (tool == ToolsManager.Tool.pen) return true;
        if (tool == ToolsManager.Tool.eraser) return true;
        if (tool == ToolsManager.Tool.bucket) return true;
        else return false;
    }

    public void setCurrentActionIndex(int index)
    {
        currentActionIndex = index;
        Debug.Log(currentActionIndex);
        foreach (GameObject action in history)
        {
            action.GetComponent<Action>().resetBackgroundColor();
        }
        history.ElementAt(currentActionIndex).GetComponent<Action>().setActiveBackgroundColor();
    }

    public Action getCurrentAction()
    {
        return history.ElementAt(currentActionIndex).GetComponent<Action>();
    }
}
