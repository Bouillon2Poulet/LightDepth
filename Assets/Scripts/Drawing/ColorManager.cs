using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private Color currentColor;
    public GameObject cursor;
    public GameObject colorWheel;

    // Start is called before the first frame update
    void Start()
    {
        setCurrentColor(Color.black);
    }

    public void setCurrentColor(Color newColor)
    {
        currentColor = newColor;
        GetComponentInChildren<CurrentColorButton>().updateColorButtonColor();
        cursor.GetComponent<UnityEngine.UI.Image>().color = currentColor;
    }

    public Color getCurrentColor()
    {
        return currentColor;
    }

    public void disableColorWheelIfEnable()
    {
        if (colorWheel.GetComponent<UnityEngine.UI.Image>().enabled)
        {
            colorWheel.GetComponent<UnityEngine.UI.Image>().enabled = false;
        }
    }
}
