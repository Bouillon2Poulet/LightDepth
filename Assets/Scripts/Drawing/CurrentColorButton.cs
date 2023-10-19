using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentColorButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject colorWheel;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // //Use this to tell when the user right-clicks on the Button
        // if (pointerEventData.button == PointerEventData.InputButton.Right)
        // {
        //     //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        //     Debug.Log(name + " Game Object Right Clicked!");
        // }

        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("colorClicked");
            colorWheel.GetComponent<UnityEngine.UI.Image>().enabled = !colorWheel.GetComponent<UnityEngine.UI.Image>().enabled;
        }
    }

    void Start()
    {
        GetComponent<UnityEngine.UI.Image>().color = GetComponentInParent<ColorManager>().currentColor;
    }
    void Update()
    {
        GetComponent<UnityEngine.UI.Image>().color = GetComponentInParent<ColorManager>().currentColor;
    }
}