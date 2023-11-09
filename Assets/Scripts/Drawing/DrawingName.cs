using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class DrawingName : MonoBehaviour, IPointerClickHandler
{
    public bool isTyping = false;
    public int lastFrameDeleted = 0;
    public int deltaFrame = 30;

    public string documentName = "newDocument";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnityEngine.UI.Text>().text = documentName;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            isTyping = true;
        }
    }

    void Update()
    {
        if (isTyping)
        {
            if (Input.anyKey)
            {
                if (Input.GetKey(KeyCode.Delete) && documentName.Length != 0)
                {
                    if (Time.frameCount > lastFrameDeleted + deltaFrame)
                    {
                        documentName = documentName.Remove(documentName.Length - 1);
                        lastFrameDeleted = Time.frameCount;

                    }
                }

                if (Input.inputString.Length != 0)
                {
                    foreach (char a in Input.inputString)
                    {
                        if (char.IsLetterOrDigit(a))
                        {
                            documentName += a;
                        }
                    }
                }
                GetComponent<UnityEngine.UI.Text>().text = documentName;
            }
        }
    }
}
