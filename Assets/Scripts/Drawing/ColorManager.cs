using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private Color currentColor;
    // Start is called before the first frame update
    void Start()
    {
        setCurrentColor(Color.black);
    }

    public void setCurrentColor(Color newColor)
    {
        currentColor = newColor;
        GetComponentInChildren<CurrentColorButton>().updateColorButtonColor();
    }

    public Color getCurrentColor()
    {
        return currentColor;
    }
}
