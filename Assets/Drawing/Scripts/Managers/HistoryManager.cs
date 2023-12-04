using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HistoryManager : MonoBehaviour
{
    public GameObject actionPrefab;
    public GameObject actionsEmptyParent;
    public LinkedList<GameObject> history;
    static int currentActionIndex = 0;

    public Scrollbar scrollbar;
    private Vector3 firstActionPosition = new Vector3(870, 75, 0);
    private Vector3 positionOffset = new Vector3(0, 33, 0);

    void Start()
    {
        history = new LinkedList<GameObject>();
    }
    public void newActionIfNecessary(ToolsManager.Tool tool, ModeManager.Mode mode)
    {
        if (toolsCreateAction(tool))
        {
            if (history.Count == 50)
            {
                Destroy(history.Last());
                history.RemoveLast();
            }
            history.AddFirst(Instantiate(actionPrefab));
            history.First().transform.SetParent(actionsEmptyParent.transform);
            history.First().GetComponent<Action>().initAction(tool, mode);

            int i = 0;
            foreach (GameObject action in history)
            {
                handleActionPosition(action, i);
                action.GetComponent<Action>().resetBackgroundColor();
                action.GetComponent<Action>().setIndex(i);
                i++;
            }
            history.First().GetComponent<Action>().setActiveBackgroundColor();
        }
    }

    public void handleActionPosition(GameObject action, int row)
    {
        action.transform.position = actionsEmptyParent.transform.position + new Vector3(0, 99 - row * positionOffset.y, 0);
    }
    public void updateCurrentAction(ActionDatas actionDatas)
    {
        history.ElementAt(currentActionIndex).GetComponent<Action>().addActionDatas(actionDatas);
    }

    public void scrollAmongActions()
    {
        int row = 0;
        float offsetY = history.Count - 6 > 0 ? Mathf.Lerp(0, (history.Count - 7) * positionOffset.y, scrollbar.value) : 0;

        foreach (GameObject action in history)
        {
            action.transform.position = actionsEmptyParent.transform.position + new Vector3(0, 99 - row * positionOffset.y, 0) + new Vector3(0, offsetY, 0);
            row++;
        }
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
