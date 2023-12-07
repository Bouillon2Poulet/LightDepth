using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Preview3D_Btn : MonoBehaviour, IPointerClickHandler
{
    public GameObject Creator3D;
    public GameObject ObjectPreviewCanvas;

    // Start is called before the first frame update    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //FIRST : SAVE
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            GetComponentInParent<SaveManager>().save();
        }
        //SECOND : GENERATE 3D MODEL FROM SAVED TEXTURE
        ObjectPreviewCanvas.SetActive(true);
        Creator3D.GetComponent<Generation3D>().generate3DModel();
    }
}
