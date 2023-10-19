using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class DrawingManager : MonoBehaviour
{
    public Vector2Int textureDimension = new Vector2Int(50, 50);
    public Material drawingMaterial;
    Texture2D drawingTexture;
    Sprite drawingSprite;
    GameObject drawingCanvas;

    Vector2 currentPoint;
    List<Vector2> PointsList;

    public GameObject drawingToolManager;

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

        Debug.Log(drawingTexture.GetPixel(25, 25).a);
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
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 hitOnTextureCoordinates = (hit.point + new Vector3(5f, 0f, 5f)) / 10f * textureDimension.x;
                currentPoint = new Vector2((int)hitOnTextureCoordinates.x, (int)hitOnTextureCoordinates.z);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                // Debug.Log(hit.point); //For now the plane goes from -5 to 5
                Vector3 hitOnTextureCoordinates = (hit.point + new Vector3(5f, 0f, 5f)) / 10f * textureDimension.x;
                // Debug.Log(hitOnTextureCoordinates);
                Vector2Int hitOnPixelCoordinates = new Vector2Int((int)hitOnTextureCoordinates.x, (int)hitOnTextureCoordinates.z);

                PointsList.Clear();
                Vector2 lastFramePoint = currentPoint;
                currentPoint = new Vector2(hitOnPixelCoordinates.x, hitOnPixelCoordinates.y);

                Vector2 drawVector = currentPoint - lastFramePoint;
                float drawVectorLength = drawVector.magnitude;
                Vector2 drawVectorNormalized = drawVector.normalized;

                // // // PointsList.Add(LastPoint);
                // // // for (int i = 0; i < drawVectorLength; i++)
                // // // {
                // // //     PointsList.Add(new Vector2((int)(LastPoint.x + i * drawVectorNormalized.x), (int)(LastPoint.y + i * drawVectorNormalized.y)));
                // // // }
                // // // PointsList.Add(NewPoint);

                if (Mathf.Abs(currentPoint.x - lastFramePoint.x) > 1 || Mathf.Abs(currentPoint.y - lastFramePoint.y) > 1)
                {
                    // Debug.Log("LastPoint : " + lastFramePoint);
                    // Debug.Log("NewPoint : " + currentPoint);
                    for (int i = 0; i < drawVectorLength; i++)
                    {
                        PointsList.Add(new Vector2((int)(lastFramePoint.x + i * drawVectorNormalized.x), (int)(lastFramePoint.y + i * drawVectorNormalized.y)));
                        Debug.Log("PointsLists[" + i + "] : " + PointsList[i]);
                    }
                }
                PointsList.Add(currentPoint);

                drawingToolManager.GetComponent<DrawingToolManager>().draw(drawingTexture, PointsList);
                // drawingToolManager.GetComponent<DrawingToolManager>().action(drawingTexture, hitOnPixelCoordinates);

                // drawingTexture.SetPixel(hitOnPixelCoordinates.x, hitOnPixelCoordinates.y, Color.black);
                drawingTexture.Apply();

                // var filePath = System.IO.Path.Combine(Application.dataPath + "/drawingTexture/", drawingTexture.name + ".png");
                // System.IO.File.WriteAllBytes(filePath, drawingTexture.EncodeToPNG());

                // drawingCanvas.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", drawingTexture);
            }
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

}
