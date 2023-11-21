using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;

public class DrawingManagerOld : MonoBehaviour
{
    //INPUT
    public Vector2 textureDimension = new Vector2(50, 50);
    public Material drawingMaterial;
    public GameObject background_texture;

    //COLOR TEXTURE
    Texture2D colorTexture;
    Texture2D heightTexture;
    Texture2D[] drawingTexturesArray;
    GameObject drawingMesh;


    Vector2 currentFramePixelPosition;
    List<Vector2> mouseHoveredPixelDeltas;
    private bool needForANewClick;
    public GameObject DrawingMeshCamera;


    //////////////MANAGER
    public GameObject drawingToolManager;
    public GameObject colorManager;
    private GameObject[] actionsHistoryManagerArray;
    public GameObject COLOR_actionHistoryManager;
    public GameObject HEIGHT_actionHistoryManager;


    public enum DrawingMode
    {
        color,
        height
    }
    public DrawingMode mode = DrawingMode.color;

    // Start is called before the first frame update
    void Start()
    {
        generateTexture(ref colorTexture, "colorTexture");
        generateTexture(ref heightTexture, "heightTexture");

        drawingTexturesArray = new Texture2D[2];
        drawingTexturesArray[0] = colorTexture;
        drawingTexturesArray[1] = heightTexture;

        generateDrawingMesh(ref drawingMesh, "drawingMesh");

        actionsHistoryManagerArray = new GameObject[2];
        actionsHistoryManagerArray[0] = COLOR_actionHistoryManager;
        actionsHistoryManagerArray[1] = HEIGHT_actionHistoryManager;


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
                        actionsHistoryManagerArray[(int)mode].GetComponent<ActionHistoryManager>().newAction(drawingToolManager.GetComponent<DrawingToolManager>().getSelectedToolType());
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

                    if (actionsHistoryManagerArray[(int)mode].GetComponent<ActionHistoryManager>().actionsHistory.Count == 0)
                    {
                        drawingToolManager.GetComponent<DrawingToolManager>().performSelectedToolAction(drawingTexturesArray[(int)mode], mouseHoveredPixelDeltas, new Action());
                    }
                    else
                    {
                        drawingToolManager.GetComponent<DrawingToolManager>().performSelectedToolAction(drawingTexturesArray[(int)mode], mouseHoveredPixelDeltas, actionsHistoryManagerArray[(int)mode].GetComponent<ActionHistoryManager>().actionsHistory.First().GetComponent<Action>());
                    }

                    drawingTexturesArray[(int)mode].Apply();
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

    void generateTexture(ref Texture2D texture, string name)
    {
        //Init texture
        texture = new Texture2D((int)textureDimension.x, (int)textureDimension.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = name,
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };
        var filePath = System.IO.Path.Combine(Application.dataPath + "/drawingTexturesTemp/", texture.name + ".png");
        fillTextureWithBlackAndFullTransparency(texture);
        System.IO.File.WriteAllBytes(filePath, texture.EncodeToPNG());
    }

    void generateDrawingMesh(ref GameObject drawingMesh, string name)
    {
        //Init canvas
        drawingMesh = GameObject.CreatePrimitive(PrimitiveType.Plane);
        drawingMesh.layer = LayerMask.NameToLayer("DrawingMesh");
        drawingMesh.transform.Rotate(new Vector3(0, 1, 0), 180);
        drawingMesh.transform.localScale = new Vector3(textureDimension.x / 50, 1, textureDimension.y / 50);
        drawingMesh.name = name;
        drawingMesh.GetComponent<MeshRenderer>().material = drawingMaterial;
        drawingMesh.GetComponent<MeshRenderer>().material.SetTexture("_DrawingTexture", colorTexture);
        drawingMesh.GetComponent<MeshRenderer>().material.SetTexture("_HeightTexture", heightTexture);
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
    void fillTextureWithBlackAndFullTransparency(Texture2D texture)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, new Color(0, 0, 0, 0));
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
        colorTexture.SetPixel((int)position.x, (int)position.y, color);
    }

    public void applyDrawingTexture()
    {
        colorTexture.Apply();
    }

    public Texture2D getColorTexture()
    {
        return colorTexture;
    }

    public Texture2D getHeightTexture()
    {
        return heightTexture;
    }

    public void switchDrawingMode()
    {
        if (mode == DrawingMode.color)
        {
            mode = DrawingMode.height;
            drawingMesh.GetComponent<MeshRenderer>().material.SetFloat("_DrawingTextureAlpha", 0.5f);
            drawingMesh.GetComponent<MeshRenderer>().material.SetFloat("_HeightTextureAlpha", 1f);
            actionsHistoryManagerArray[0].SetActive(false);
            actionsHistoryManagerArray[1].SetActive(true);
        }
        else if (mode == DrawingMode.height)
        {
            mode = DrawingMode.color;
            drawingMesh.GetComponent<MeshRenderer>().material.SetFloat("_DrawingTextureAlpha", 1f);
            drawingMesh.GetComponent<MeshRenderer>().material.SetFloat("_HeightTextureAlpha", 0f);
            actionsHistoryManagerArray[0].SetActive(true);
            actionsHistoryManagerArray[1].SetActive(false);
        }
        GetComponentsInChildren<DrawingModeBtn>()[0].switchColor();
        GetComponentsInChildren<DrawingModeBtn>()[1].switchColor();
    }
}