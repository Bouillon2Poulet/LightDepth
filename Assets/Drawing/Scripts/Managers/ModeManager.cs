using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public enum Mode
    {
        color,
        height
    }

    public GameObject mode_first;
    public GameObject mode_second;

    public GameObject colors_color;
    public GameObject colors_height;

    public Sprite mode_color_first;
    public Sprite mode_color_second;
    public Sprite mode_height_first;
    public Sprite mode_height_second;
    public Sprite background1;
    public Sprite background2;
    public LayersManager layersManager;
    public UnityEngine.UI.Image backgroundTexture;

    public Mode currentMode = Mode.color;

    public void Start()
    {
        colors_height.SetActive(false);
    }
    public void swapCurrentMode()
    {
        Debug.Log("swap " + currentMode);
        if (currentMode == Mode.color)
        {
            setCurrentMode(Mode.height);
        }
        else
        {
            setCurrentMode(Mode.color);
        }
    }

    // public void switchModeColors()
    // {
    //     if (currentMode == Mode.color)
    //     {
    //         mode_first.GetComponent<UnityEngine.UI.Image>().sprite = mode_color_first;
    //         mode_second.GetComponent<UnityEngine.UI.Image>().sprite = mode_height_second;
    //         setCurrentMode(Mode.height);
    //     }
    //     else
    //     {
    //         mode_first.GetComponent<UnityEngine.UI.Image>().sprite = mode_height_first;
    //         mode_second.GetComponent<UnityEngine.UI.Image>().sprite = mode_color_second;
    //         setCurrentMode(Mode.color);
    //     }
    // }

    public void setCurrentMode(Mode mode)
    {
        currentMode = mode;
        if (currentMode == Mode.color)
        {
            Debug.Log("swap to color");
            mode_first.GetComponent<UnityEngine.UI.Image>().sprite = mode_color_first;
            mode_second.GetComponent<UnityEngine.UI.Image>().sprite = mode_height_second;
            colors_color.SetActive(true);
            colors_height.SetActive(false);
            backgroundTexture.sprite = background1;
            layersManager.setCurrentLayerIndex(1);
        }
        else if (currentMode == Mode.height)
        {
            Debug.Log("swap to height " + currentMode);
            mode_first.GetComponent<UnityEngine.UI.Image>().sprite = mode_height_first;
            mode_second.GetComponent<UnityEngine.UI.Image>().sprite = mode_color_second;
            colors_height.SetActive(true);
            colors_color.SetActive(false);
            backgroundTexture.sprite = background2;
            layersManager.setCurrentLayerIndex(2);
        }
        layersManager.updateLayersVisibilityAccordingToCurrentMode();
    }

    public int getModeIndex()
    {
        if (currentMode == Mode.color)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}
