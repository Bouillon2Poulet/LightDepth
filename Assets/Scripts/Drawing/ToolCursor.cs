using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCursor : MonoBehaviour
{
    public float offset = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(offset, -offset, 0);
    }
    public void setToolCursorTexture(Sprite sprite)
    {
        GetComponent<UnityEngine.UI.Image>().sprite = sprite;
    }
}
