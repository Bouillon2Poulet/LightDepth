using UnityEngine;
using UnityEngine.EventSystems;

public class preview3D_button : MonoBehaviour, IPointerClickHandler
{
    public bool isShrinkButton = false;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (isShrinkButton)
            {
                GetComponentInParent<preview3D_manager>().shrinkPreview();
            }
            else
            {
                GetComponentInParent<preview3D_manager>().extendPreview();
            }
        }
    }
}
