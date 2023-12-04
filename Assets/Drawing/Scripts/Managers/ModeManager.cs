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

    public Mode currentMode = Mode.color;

    public void Start()
    {
        colors_height.SetActive(false);
    }
    public void swapCurrentMode()
    {
        if (currentMode == Mode.color)
        {
            currentMode = Mode.height;
            colors_height.SetActive(true);
            colors_color.SetActive(false);
        }
        else if (currentMode == Mode.height)
        {
            currentMode = Mode.color;
            colors_color.SetActive(true);
            colors_height.SetActive(false);
        }
        switchModeColors();
    }

    public void switchModeColors()
    {
        if (currentMode == Mode.color)
        {
            mode_first.GetComponent<UnityEngine.UI.Image>().sprite = mode_color_first;
            mode_second.GetComponent<UnityEngine.UI.Image>().sprite = mode_height_second;
        }
        else
        {
            mode_first.GetComponent<UnityEngine.UI.Image>().sprite = mode_height_first;
            mode_second.GetComponent<UnityEngine.UI.Image>().sprite = mode_color_second;
        }
    }
}
