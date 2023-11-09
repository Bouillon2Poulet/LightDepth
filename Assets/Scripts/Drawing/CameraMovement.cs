using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float dragSpeed = 2;

    private Vector3 dragOrigin;
    private float lastOrthographicSize;

    public void Start()
    {
        updateDragOriginAndLastOrthographicSize();
    }
    public void Update()
    {
        drag();
    }
    public void drag()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            dragOrigin = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            Vector3 difference = dragOrigin - GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            transform.position += difference;
        }
    }

    public void zoom()
    {
        //NE PASSE PAS ICI
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("lastOrthographicSize" + lastOrthographicSize);
        //     lastOrthographicSize = GetComponent<Camera>().orthographicSize;
        //     dragOrigin = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        //     return;
        // }
        if (Input.GetMouseButton(0))
        {
            // Debug.Log("DRAGORIGIN POSITION " + dragOrigin);
            // Debug.Log("CLICK POSITION " + Input.mousePosition);
            // Debug.Log("DIFFERENCE VECTOR" + difference);
            Vector3 difference = Input.mousePosition - dragOrigin;

            float newSize = lastOrthographicSize - difference.x / 20;
            if (newSize > 1 && newSize < 15)
            {
                GetComponent<Camera>().orthographicSize = newSize;
            }

            return;
        }
    }

    public void updateDragOriginAndLastOrthographicSize()
    {
        dragOrigin = Input.mousePosition;
        lastOrthographicSize = GetComponent<Camera>().orthographicSize;
    }
}