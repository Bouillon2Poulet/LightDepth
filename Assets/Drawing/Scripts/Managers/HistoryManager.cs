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
    public LayersManager layersManager;

    public Scrollbar scrollbar;
    private Vector3 firstActionPosition = new Vector3(870, 75, 0);
    private Vector3 positionOffset = new Vector3(0, 33, 0);

    public Button redoButton;
    public Button undoButton;

    void Start()
    {
        history = new LinkedList<GameObject>();
        setCurrentActionIndex(0);
    }
    public void newActionIfNecessary(ToolsManager.Tool tool, ModeManager.Mode mode)
    {
        if (toolsCreateAction(tool))
        {
            if (currentActionIndex != 0)
            {
                for (int j = 0; j < currentActionIndex; j++)
                {
                    Destroy(history.First());
                    history.RemoveFirst();
                }
            }
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
                // action.GetComponent<Action>().printActionDatas();
                handleActionPosition(action, i);
                action.GetComponent<Action>().resetBackgroundColor();
                action.GetComponent<Action>().setIndex(i);
                i++;
            }
            history.First().GetComponent<Action>().setActiveBackgroundColor();
            setCurrentActionIndex(0);
        }
    }

    public void handleActionPosition(GameObject action, int row)
    {
        action.transform.position = actionsEmptyParent.transform.position + new Vector3(0, 99 - row * positionOffset.y, 0);
    }
    public void updateCurrentAction(List<Action.ActionData> actionDatas)
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
        Debug.Log("currentActionIndex" + currentActionIndex);
        foreach (GameObject action in history)
        {
            action.GetComponent<Action>().resetBackgroundColor();
        }

        if (currentActionIndex != history.Count && history.Count != 0)
        {
            history.ElementAt(currentActionIndex).GetComponent<Action>().setActiveBackgroundColor();
        }

        manageUndoRedoButtons();
    }

    public void moveToAction(int actionIndex)
    {
        if (actionIndex < currentActionIndex)
        {
            RedoAction(currentActionIndex - actionIndex);
        }
        else if (actionIndex > currentActionIndex)
        {
            UndoAction(actionIndex - currentActionIndex);
        }
    }

    public void manageUndoRedoButtons()
    {
        if (history.Count == 0)
        {
            undoButton.interactable = false;
            redoButton.interactable = false;
            return;
        }

        if (currentActionIndex == history.Count)
        {
            Debug.Log("tout au bout");
            undoButton.interactable = false;
            redoButton.interactable = true;
            return;
        }

        if (currentActionIndex == 0)
        {
            undoButton.interactable = true;
            redoButton.interactable = false;
            return;
        }

        undoButton.interactable = true;
        redoButton.interactable = true;
    }

    public Action getCurrentAction()
    {
        if (history.Count > 0)
        {
            return history.ElementAt(currentActionIndex).GetComponent<Action>();
        }
        else return new Action();
    }

    public void UndoAction(int numberOfUndo)
    {
        Debug.Log("numberOfUndo" + numberOfUndo);
        for (int i = 0; i < numberOfUndo; i++)
        {
            if (history.ElementAt(currentActionIndex + i).GetComponent<Action>().mode == ModeManager.Mode.color)
            {
                for (int j = history.ElementAt(currentActionIndex + i).GetComponent<Action>().actionDatas.Count; j > 0; j--)
                {
                    Action.ActionData data = history.ElementAt(currentActionIndex + i).GetComponent<Action>().actionDatas[j - 1];
                    layersManager.ColorMap.texture.SetPixel(data.position.x, data.position.y, data.colorBeforeAction);
                }
                layersManager.ColorMap.texture.Apply();
            }
            else
            {
                for (int j = history.ElementAt(currentActionIndex + i).GetComponent<Action>().actionDatas.Count; j > 0; j--)
                {
                    Action.ActionData data = history.ElementAt(currentActionIndex + i).GetComponent<Action>().actionDatas[j - 1];
                    layersManager.HeightMap.texture.SetPixel(data.position.x, data.position.y, data.colorBeforeAction);
                }
                layersManager.HeightMap.texture.Apply();
            }
        }
        setCurrentActionIndex(currentActionIndex + numberOfUndo);
    }

    public void RedoAction(int numberOfRedo)
    {
        Debug.Log("numberOfRedo" + numberOfRedo);
        for (int i = 1; i < numberOfRedo + 1; i++)
        {
            if (history.ElementAt(currentActionIndex - i).GetComponent<Action>().mode == ModeManager.Mode.color)
            {
                for (int j = 0; j < history.ElementAt(currentActionIndex - i).GetComponent<Action>().actionDatas.Count; j++)
                {
                    Action.ActionData data = history.ElementAt(currentActionIndex - i).GetComponent<Action>().actionDatas[j];
                    layersManager.ColorMap.texture.SetPixel(data.position.x, data.position.y, data.colorAfterAction);
                }
                layersManager.ColorMap.texture.Apply();
            }
            else
            {
                for (int j = 0; j < history.ElementAt(currentActionIndex - i).GetComponent<Action>().actionDatas.Count; j++)
                {
                    Action.ActionData data = history.ElementAt(currentActionIndex - i).GetComponent<Action>().actionDatas[j];
                    layersManager.HeightMap.texture.SetPixel(data.position.x, data.position.y, data.colorAfterAction);
                }
                layersManager.HeightMap.texture.Apply();
            }
        }
        setCurrentActionIndex(currentActionIndex - numberOfRedo);
    }
}
