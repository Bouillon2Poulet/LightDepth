using UnityEngine;
using UnityEngine.EventSystems;

public class Loader3DDragger : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector3 startPosition;
    private Vector3 initialMousePosition;
    private bool animation = false;
    private bool needForANewClick = true;

    public float maxXOffset = 85f;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("!");
            initialMousePosition = Input.mousePosition;
            needForANewClick = false;
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if (animation == false && needForANewClick == false)
        {
            float x = Mathf.Clamp(initialMousePosition.x - Input.mousePosition.x, 0, maxXOffset);

            if (transform.position.x > startPosition.x - maxXOffset && transform.position.x <= startPosition.x)
            {
                Debug.Log("x:" + x);
                transform.position = startPosition - new Vector3(x, 0, 0);
            }

            // if (transform.position.x <= startPosition.x)
            // {
            //     transform.position = startPosition;
            // }

            else if (transform.position.x <= startPosition.x - maxXOffset)
            {
                Debug.Log("LOADING");
                GetComponentInParent<Loader3DManager>().load3DModel();
                animation = true;
            }
        }
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (animation)
        {
            transform.position += new Vector3(0.2f, 0, 0);
        }
        if (transform.position.x >= startPosition.x && animation == true)
        {
            transform.position = startPosition;
            animation = false;
            needForANewClick = true;
        }
    }
}
