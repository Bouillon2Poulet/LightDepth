using UnityEngine;

public class LayerVisibilityButton : MonoBehaviour
{
    public Sprite visibleSprite;
    public Sprite invisibleSprite;

    public void Start()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = GetComponentInParent<Layer>().visible ? visibleSprite : invisibleSprite;
    }
    public void swapSprite()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = (GetComponent<UnityEngine.UI.Image>().sprite == visibleSprite) ? invisibleSprite : visibleSprite;
    }

    public void setSpriteWithVisibility(bool visibility)
    {
        GetComponent<UnityEngine.UI.Image>().sprite = visibility == true ? visibleSprite : invisibleSprite;
    }
}
