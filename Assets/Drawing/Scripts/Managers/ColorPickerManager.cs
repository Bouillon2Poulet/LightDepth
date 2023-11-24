using UnityEngine;
using HSVPicker;

public class ColorPickerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer renderer;
    public ColorPicker picker;
    void Start()
    {
        picker.onValueChanged.AddListener(color =>
        {
            renderer.material.color = color;
        });
        renderer.material.color = picker.CurrentColor;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
