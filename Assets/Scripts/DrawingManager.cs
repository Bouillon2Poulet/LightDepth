using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class DrawingManager : MonoBehaviour
{
    public Vector2Int textureDimension = new Vector2Int(50, 50);
    public Material drawingMaterial;
    Texture2D drawingTexture;
    Sprite drawingSprite;
    GameObject drawingCanvas;

    // GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        drawingTexture = new Texture2D(textureDimension.x, textureDimension.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = "drawingTexture",
            filterMode = FilterMode.Point
        };
        var filePath = System.IO.Path.Combine(Application.dataPath + "/drawingTexture/", drawingTexture.name + ".png");

        // fillTextureWithWhite(drawingTexture);

        // Debug.Log(drawingTexture.GetPixel(25, 25));
        // Debug.Log(filePath);
        System.IO.File.WriteAllBytes(filePath, drawingTexture.EncodeToPNG());

        drawingCanvas = GameObject.CreatePrimitive(PrimitiveType.Plane);
        drawingCanvas.transform.Rotate(new Vector3(0, 1, 0), 180);
        drawingCanvas.name = "drawingCanvas";
        drawingCanvas.GetComponent<MeshRenderer>().material = drawingMaterial;
        drawingCanvas.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", drawingTexture);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                // Debug.Log(hit.point); //For now the plane goes from -5 to 5
                Vector3 hitOnTextureCoordinates = (hit.point + new Vector3(5f, 0f, 5f)) / 10f * textureDimension.x;
                Debug.Log(hitOnTextureCoordinates);
                Vector2Int hitOnPixelCoordinates = new Vector2Int((int)hitOnTextureCoordinates.x, (int)hitOnTextureCoordinates.z);
                drawingTexture.SetPixel(hitOnPixelCoordinates.x, hitOnPixelCoordinates.y, Color.black);
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
    }
}
