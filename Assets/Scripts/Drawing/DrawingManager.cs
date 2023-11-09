using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;

public class DrawingManager : MonoBehaviour
{
    //INPUT
    public Vector2 textureDimension = new Vector2(50, 50);
    public Material drawingMaterial;
    Texture2D drawingTexture;
    public GameObject background_texture;

    GameObject drawingMesh;
    Vector2 currentFramePixelPosition;
    List<Vector2> mouseHoveredPixelDeltas;
    private bool needForANewClick;
    public GameObject DrawingMeshCamera;


    //////////////MANAGER
    public GameObject drawingToolManager;
    public GameObject colorManager;
    public GameObject actionHistoryManager;


    // Start is called before the first frame update
    void Start()
    {
        //Init texture
        drawingTexture = new Texture2D((int)textureDimension.x, (int)textureDimension.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = "drawingTexture",
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };
        var filePath = System.IO.Path.Combine(Application.dataPath + "/drawingTexture/", drawingTexture.name + ".png");
        fillTextureWithWhiteAndFullTransparency(drawingTexture);
        System.IO.File.WriteAllBytes(filePath, drawingTexture.EncodeToPNG());

        //Init canvas
        drawingMesh = GameObject.CreatePrimitive(PrimitiveType.Plane);
        drawingMesh.layer = LayerMask.NameToLayer("DrawingMesh");
        drawingMesh.transform.Rotate(new Vector3(0, 1, 0), 180);
        drawingMesh.transform.localScale = new Vector3(textureDimension.x / 50, 1, textureDimension.y / 50);
        drawingMesh.name = "drawingMesh";
        drawingMesh.GetComponent<MeshRenderer>().material = drawingMaterial;
        drawingMesh.GetComponent<MeshRenderer>().material.SetTexture("_Texture", drawingTexture);

        mouseHoveredPixelDeltas = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!background_texture.GetComponent<IsOvered>().isOvered)
        {
            needForANewClick = true;
        }
        if (!Input.GetKey(KeyCode.LeftAlt) && background_texture.GetComponent<IsOvered>().isOvered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                colorManager.GetComponent<ColorManager>().disableColorWheelIfEnable();
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    needForANewClick = false;
                    mouseHoveredPixelDeltas.Clear();
                    if (drawingToolManager.GetComponent<DrawingToolManager>().selectedToolCreatesAction())
                    {
                        actionHistoryManager.GetComponent<ActionHistoryManager>().newAction(drawingToolManager.GetComponent<DrawingToolManager>().getSelectedToolType());
                    }
                    currentFramePixelPosition = drawingMeshPositionToTexturePosition(hit.point);

                    DrawingMeshCamera.GetComponent<CameraMovement>().updateDragOriginAndLastOrthographicSize();
                }
            }
            else if (Input.GetMouseButton(0) && !needForANewClick)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    // Debug.Log(hit.collider.name);
                    //VERIFIER QUE LA FONCTION OnPointerClick(PointerEventData pointerEventData) de tous les bouttons n'a pas été activé
                    calculateNewHoveredPixelDeltas(hit.point);

                    if (actionHistoryManager.GetComponent<ActionHistoryManager>().actionHistory.Count == 0)
                    {
                        drawingToolManager.GetComponent<DrawingToolManager>().performSelectedToolAction(drawingTexture, mouseHoveredPixelDeltas, new Action());
                    }
                    else
                    {
                        drawingToolManager.GetComponent<DrawingToolManager>().performSelectedToolAction(drawingTexture, mouseHoveredPixelDeltas, actionHistoryManager.GetComponent<ActionHistoryManager>().actionHistory.First().GetComponent<Action>());
                    }

                    drawingTexture.Apply();

                }
            }

        }
        // else if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        //     if (selectedObject != null)
        //     {
        //         Debug.Log("Selected UI Element Name: " + selectedObject.name);
        //     }
        // }

    }

    void fillTextureWithWhiteAndFullTransparency(Texture2D texture)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }
        texture.Apply();
    }

    Vector2 drawingMeshPositionToTexturePosition(Vector3 position)
    {
        position /= textureDimension.x / 50;
        Vector3 hitOnTextureCoordinates = (position + new Vector3(5f, 0f, 5f)) / 10f * textureDimension.x;
        return new Vector2((int)hitOnTextureCoordinates.x, (int)hitOnTextureCoordinates.z);
    }

    void calculateNewHoveredPixelDeltas(Vector3 hitPosition)
    {
        Vector2 lastFramePoint = currentFramePixelPosition;
        currentFramePixelPosition = drawingMeshPositionToTexturePosition(hitPosition);
        Vector2 drawVector = currentFramePixelPosition - lastFramePoint;
        mouseHoveredPixelDeltas.Clear();
        float drawVectorLength = drawVector.magnitude;
        Vector2 drawVectorNormalized = drawVector.normalized;
        mouseHoveredPixelDeltas.Add(lastFramePoint);
        if (Mathf.Abs(currentFramePixelPosition.x - lastFramePoint.x) > 1 || Mathf.Abs(currentFramePixelPosition.y - lastFramePoint.y) > 1)
        {
            for (int i = 0; i < drawVectorLength; i++)
            {
                mouseHoveredPixelDeltas.Add(new Vector2((int)(lastFramePoint.x + i * drawVectorNormalized.x), (int)(lastFramePoint.y + i * drawVectorNormalized.y)));
            }
        }
        mouseHoveredPixelDeltas.Add(currentFramePixelPosition);
    }

    public void setDrawingTexturePixel(Vector2 position, Color color)
    {
        drawingTexture.SetPixel((int)position.x, (int)position.y, color);
    }

    public void applyDrawingTexture()
    {
        drawingTexture.Apply();
    }

    public Texture2D getDrawingTexture()
    {
        return drawingTexture;
    }
}