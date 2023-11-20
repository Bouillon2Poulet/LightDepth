using UnityEngine;
using UnityEngine.EventSystems;

public class color_palette_button : MonoBehaviour, IPointerClickHandler
{
    public class ColorAndCoordinates
    {
        public Color color;
        public Vector2 coordinates;

    }
    public ColorAndCoordinates colorAndCoordinates;
    public GameObject outline;
    public int nameAsIndex;

    public Vector2 coordinatesOnSpectrumTexture;
    // Start is called before the first frame update
    void Start()
    {
        // color = GetComponent<UnityEngine.UI.Image>().color;
        GetComponent<UnityEngine.UI.Image>().color = colorAndCoordinates.color;
        nameAsIndex = int.Parse(name.Split("palette_")[1]) - 1;
        if (GetComponentInParent<colors_manager>().currentPaletteIndex == nameAsIndex)
            outline.GetComponent<UnityEngine.UI.Image>().color = GetComponentInParent<colors_manager>().selectedOutlineColor;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            GetComponentInParent<colors_manager>().SetCurrentPaletteIndex(nameAsIndex);
            outline.GetComponent<UnityEngine.UI.Image>().color = GetComponentInParent<colors_manager>().selectedOutlineColor;
            if (nameAsIndex == 0 || nameAsIndex == 1)
            {
                transform.SetSiblingIndex(15);
            }
        }
    }

    public void resetOutlineColor()
    {
        outline.GetComponent<UnityEngine.UI.Image>().color = GetComponentInParent<colors_manager>().defaultOutlineColor;
    }

    public void setColorAndCoordinates(ColorAndCoordinates inputColorAndCoordinates)
    {
        colorAndCoordinates = inputColorAndCoordinates;
        GetComponent<UnityEngine.UI.Image>().color = colorAndCoordinates.color;
    }
}
