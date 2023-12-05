using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preview3D_manager : MonoBehaviour
{
    public GameObject preview_mini;
    public GameObject preview_medium;
    public GameObject preview_maxi;
    List<GameObject> previews;
    public enum Size
    {
        mini,
        medium,
        maxi
    }
    public Size currentSize = Size.mini;
    // Start is called before the first frame update

    public void Start()
    {
        previews = new List<GameObject>{
            preview_mini,
            preview_medium,
            preview_maxi
        };
    }
    public void extendPreview()
    {
        currentSize++;
        hideAllPreviews();
        showCurrentPreview();
    }
    public void shrinkPreview()
    {
        currentSize--;
        hideAllPreviews();
        showCurrentPreview();
    }
    public void hideAllPreviews()
    {
        foreach (GameObject preview in previews)
        {
            preview.SetActive(false);
        }
    }
    public void showCurrentPreview()
    {
        // Debug.Log((int)currentSize);
        previews[(int)currentSize].SetActive(true);
    }
}
