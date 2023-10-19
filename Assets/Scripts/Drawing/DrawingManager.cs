using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class DrawingManager : MonoBehaviour
{
    public Vector2Int textureDimension = new Vector2Int(50, 50);
    public Material drawingMaterial;
    public Texture2D drawingTexture;
    Sprite drawingSprite;
    GameObject drawingCanvas;

    Vector2 currentFramePoint;
    List<Vector2> PointsList;


    //////////////:MANAGER
    public GameObject drawingToolManager;
    public GameObject colorManager;
    public GameObject actionHistoryManager;



    // Start is called before the first frame update
    void Start()
    {
        PointsList = new List<Vector2>();

        drawingTexture = new Texture2D(textureDimension.x, textureDimension.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = "drawingTexture",
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };
        var filePath = System.IO.Path.Combine(Application.dataPath + "/drawingTexture/", drawingTexture.name + ".png");

        // fillTextureWithWhite(drawingTexture);
        fillTextureWithWhiteAndFullTransparency(drawingTexture);

        // Debug.Log(drawingTexture.GetPixel(25, 25).a);
        // Debug.Log(filePath);
        System.IO.File.WriteAllBytes(filePath, drawingTexture.EncodeToPNG());

        drawingCanvas = GameObject.CreatePrimitive(PrimitiveType.Plane);
        drawingCanvas.transform.Rotate(new Vector3(0, 1, 0), 180);
        drawingCanvas.name = "drawingCanvas";
        drawingCanvas.GetComponent<MeshRenderer>().material = drawingMaterial;
        drawingCanvas.GetComponent<MeshRenderer>().material.SetTexture("_Texture", drawingTexture);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            PointsList.Clear();
            if (actionHistoryManager.GetComponent<ActionHistoryManager>().actionHistory.Count > 0)
            {
                actionHistoryManager.GetComponent<ActionHistoryManager>().actionHistory.First().GetComponent<Action>().printActionDatas();
            }
            addNewCurrentFramePointAndAction();
        }
        else if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector2 lastFramePoint = currentFramePoint;
                updateCurrentFramePoints(hit);
                Vector2 drawVector = currentFramePoint - lastFramePoint;


                updatePointsList(lastFramePoint, drawVector);
                Debug.Log(actionHistoryManager.GetComponent<ActionHistoryManager>().actionHistory.Count);
                drawingToolManager.GetComponent<DrawingToolManager>().action(drawingTexture, PointsList, colorManager.GetComponent<ColorManager>().currentColor, actionHistoryManager.GetComponent<ActionHistoryManager>().actionHistory.First().GetComponent<Action>());
                drawingTexture.Apply();




                // var filePath = System.IO.Path.Combine(Application.dataPath + "/drawingTexture/", drawingTexture.name + ".png");
                // System.IO.File.WriteAllBytes(filePath, drawingTexture.EncodeToPNG());

                // drawingCanvas.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", drawingTexture);
            }
            // if (Input.GetMouseButtonUp(0))
            // {
            //     Debug.Log("!!");
            //     actionHistoryManager.GetComponent<ActionHistoryManager>().actionHistory.First().printActionDatas();
            // }
        }
    }
    void fillTextureWithWhite(Texture2D texture)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
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

    void addNewCurrentFramePointAndAction()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Vector3 hitOnTextureCoordinates = (hit.point + new Vector3(5f, 0f, 5f)) / 10f * textureDimension.x;
            currentFramePoint = new Vector2((int)hitOnTextureCoordinates.x, (int)hitOnTextureCoordinates.z);

            actionHistoryManager.GetComponent<ActionHistoryManager>().newAction();
            // Debug.Log(actionHistory.First());
        }
    }

    void updatePointsList(Vector2 lastFramePoint, Vector2 drawVector)
    {
        PointsList.Clear();
        float drawVectorLength = drawVector.magnitude;
        Vector2 drawVectorNormalized = drawVector.normalized;
        PointsList.Add(lastFramePoint);
        if (Mathf.Abs(currentFramePoint.x - lastFramePoint.x) > 1 || Mathf.Abs(currentFramePoint.y - lastFramePoint.y) > 1)
        {
            for (int i = 0; i < drawVectorLength; i++)
            {
                PointsList.Add(new Vector2((int)(lastFramePoint.x + i * drawVectorNormalized.x), (int)(lastFramePoint.y + i * drawVectorNormalized.y)));
            }
        }
        PointsList.Add(currentFramePoint);
    }

    void updateCurrentFramePoints(RaycastHit hit)
    {
        Vector3 hitOnTextureCoordinates = (hit.point + new Vector3(5f, 0f, 5f)) / 10f * textureDimension.x;
        Vector2Int hitOnPixelCoordinates = new Vector2Int((int)hitOnTextureCoordinates.x, (int)hitOnTextureCoordinates.z);
        currentFramePoint = new Vector2(hitOnPixelCoordinates.x, hitOnPixelCoordinates.y);
    }
}
