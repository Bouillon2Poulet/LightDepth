using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPaletteButton : MonoBehaviour, IPointerClickHandler
{
    public struct ColorAndCoordinates
    {
        public Color color;
        public Vector2 coordinates;

    }
    public ColorAndCoordinates colorAndCoordinates;
    public GameObject outline;
    public AssemblyDefinitionReferenceAsset ManagersReference;
    public ColorsManager colorsManager;
    public int nameAsIndex;

    public Vector2 coordinatesOnSpectrumTexture;
    // Start is called before the first frame update
    public void Start()
    {
        // color = GetComponent<UnityEngine.UI.Image>().color;
        nameAsIndex = int.Parse(name.Split("palette_")[1]) - 1;
        colorsManager = GetComponentInParent<ColorsManager>();
        if (colorsManager.currentPaletteIndex == nameAsIndex)
            outline.GetComponent<UnityEngine.UI.Image>().color = colorsManager.selectedOutlineColor;
        else
        {
            outline.GetComponent<UnityEngine.UI.Image>().color = colorsManager.defaultOutlineColor;
        }
    }

    public void Update()
    {
        GetComponent<UnityEngine.UI.Image>().color = colorAndCoordinates.color;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            colorsManager.SetCurrentPaletteIndex(nameAsIndex);
            outline.GetComponent<UnityEngine.UI.Image>().color = colorsManager.selectedOutlineColor;
            if (nameAsIndex == 0 || nameAsIndex == 1)
            {
                transform.SetSiblingIndex(15);
            }
        }
    }

    public void resetOutlineColor()
    {
        outline.GetComponent<UnityEngine.UI.Image>().color = colorsManager.defaultOutlineColor;
    }

    public void setColorAndCoordinates(ColorAndCoordinates inputColorAndCoordinates)
    {
        colorAndCoordinates = inputColorAndCoordinates;
        GetComponent<UnityEngine.UI.Image>().color = colorAndCoordinates.color;
    }
}
