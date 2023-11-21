using UnityEngine;
using UnityEngine.EventSystems;

public class IsOvered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOvered = false;
    // Start is called before the first frame update  //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOvered = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isOvered = false;
    }
}
