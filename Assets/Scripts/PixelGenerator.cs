using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGenerator : MonoBehaviour
{
    public Texture2D image;
    Vector2 imgSize;
    Vector2 areaDimension;
    public float areaWidth = 1f;
    float pixelWidth;
    public float heightFactor = 1;
    float offset;

    public bool wall = true;
    public bool whiteWall = true;
    public bool allWhite = false;
    public bool invert = false;
    int invertFactor;

    private struct pixelStruct
    {
        public GameObject pixel;
        public GameObject topWall;
        public GameObject leftWall;
        public GameObject bottomWall;
        public GameObject rightWall;
        public float pixelHeight;
        public Material pixelMaterial;
        public bool isTransparent;
    }

    Shader defaultShader;

    //ARRAYS
    pixelStruct[] pixelArray;
    int pixelCount;

    // Start is called before the first frame update
    void Start()
    {
        defaultShader = Shader.Find("Custom/DoubleSidedShader");
        invertFactor = invert ? -1 : 1;
        Vector2Int imgSizeTemp = GetTextureDimensions.GetDimensions(Application.dataPath + "/Textures/" + image.name + ".png");
        imgSize.x = imgSizeTemp.x;
        imgSize.y = imgSizeTemp.y;
        pixelArray = new pixelStruct[(int)(imgSize.x * imgSize.y)];

        int maxImgSize = Mathf.Max((int)imgSize.x, (int)imgSize.y);
        float ratio = maxImgSize / areaWidth;
        areaDimension = imgSize / ratio;
        pixelWidth = areaWidth / maxImgSize;
        offset = pixelWidth * 10f;

        pixelCount = 0;
        for (int i = 0; i < imgSize.y; i++)
        {
            for (int j = 0; j < imgSize.x; j++)
            {
                //PIXEL
                createPixel(i, j);

                //WALLS
                if (wall)
                {
                    createTopWall();
                    createLeftWall();
                }

                //BOTTOM WALL IF NECESSARY
                if (i == imgSize.y - 1 && wall)
                {
                    createBottomWall();
                }
                pixelCount++;
            }
            //RIGHT WALL AT END OF LINE
            if (wall) createRightWall();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void createPixel(int i, int j)
    {
        //Creation
        pixelArray[pixelCount].pixel = GameObject.CreatePrimitive(PrimitiveType.Plane);
        pixelArray[pixelCount].pixel.name = "pixel_" + pixelCount;
        pixelArray[pixelCount].isTransparent = image.GetPixel(j, (int)imgSize.y - i - 1).a == 0;


        //Position&Scale
        pixelArray[pixelCount].pixel.transform.parent = gameObject.transform;
        pixelArray[pixelCount].pixelHeight = pixelArray[pixelCount].isTransparent ? 0 : image.GetPixel(j, (int)imgSize.y - i - 1).grayscale * heightFactor; //Start at bottom left
        pixelArray[pixelCount].pixel.transform.position = new Vector3(-1f * (areaDimension.x * 10 / 2f) + (j + 0.5f) * offset, invertFactor * pixelArray[pixelCount].pixelHeight, (areaDimension.y * 10 / 2f) - (i + 0.5f) * offset);
        pixelArray[pixelCount].pixel.transform.localScale = new Vector3(pixelWidth, 1, pixelWidth);

        //Material
        pixelArray[pixelCount].pixelMaterial = new Material(defaultShader);
        pixelArray[pixelCount].pixelMaterial.color = image.GetPixel(j, (int)imgSize.y - i - 1);
        if (!allWhite) pixelArray[pixelCount].pixel.GetComponent<Renderer>().material = pixelArray[pixelCount].pixelMaterial;

        if (pixelArray[pixelCount].isTransparent) pixelArray[pixelCount].pixel.GetComponent<MeshRenderer>().enabled = false;
    }
    void createTopWall()
    {
        //Creation
        pixelArray[pixelCount].topWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
        pixelArray[pixelCount].topWall.name = "topWall_" + pixelCount;

        //Calculation
        float topPixelYPosition = (getTopPixelIndex(pixelCount) < 0) ? 0 : pixelArray[getTopPixelIndex(pixelCount)].pixelHeight;
        float topWallHeight = Mathf.Abs(pixelArray[pixelCount].pixelHeight - topPixelYPosition);
        bool isCurrentPixelOverTopPixel = (pixelArray[pixelCount].pixelHeight >= topPixelYPosition) ? true : false;
        float YDirection = isCurrentPixelOverTopPixel ? -1f : 1f;

        //Position&Scale
        pixelArray[pixelCount].topWall.transform.position = pixelArray[pixelCount].pixel.transform.position + new Vector3(0f, invertFactor * (YDirection * topWallHeight / 2f), offset / 2f);
        pixelArray[pixelCount].topWall.transform.localScale = new Vector3(pixelWidth, 1, topWallHeight / 10f);

        //Rotation?
        float YRotationUp = (!isCurrentPixelOverTopPixel) ? 180f : 0f;
        pixelArray[pixelCount].topWall.transform.Rotate(new Vector3(90, YRotationUp, 0), Space.World);

        //Color?
        if (!whiteWall)
        {
            pixelArray[pixelCount].topWall.GetComponent<Renderer>().material = isCurrentPixelOverTopPixel ? pixelArray[pixelCount].pixelMaterial : pixelArray[getTopPixelIndex(pixelCount)].pixelMaterial;
        }
    }

    void createLeftWall()
    {
        //Creation
        pixelArray[pixelCount].leftWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
        pixelArray[pixelCount].leftWall.name = "leftWall_" + pixelCount;

        //Calculation
        float leftPixelYPosition = (pixelCount % imgSize.x == 0) ? 0 : pixelArray[pixelCount - 1].pixelHeight;
        float leftWallHeight = Mathf.Abs(pixelArray[pixelCount].pixelHeight - leftPixelYPosition);
        bool isCurrentPixelOverLeftPixel = (pixelArray[pixelCount].pixelHeight >= leftPixelYPosition) ? true : false;
        float YDirection = isCurrentPixelOverLeftPixel ? -1f : 1f;

        //Position&Scale
        pixelArray[pixelCount].leftWall.transform.position = pixelArray[pixelCount].pixel.transform.position + new Vector3(-offset / 2f, invertFactor * (YDirection * leftWallHeight / 2f), 0);
        pixelArray[pixelCount].leftWall.transform.localScale = new Vector3(pixelWidth, 1, leftWallHeight / 10f);

        //Rotation?
        float YRotationLeft = (!isCurrentPixelOverLeftPixel) ? 180f : 0f;
        pixelArray[pixelCount].leftWall.transform.Rotate(new Vector3(90, YRotationLeft, 90), Space.World);

        //Color?
        if (!whiteWall)
        {
            pixelArray[pixelCount].leftWall.GetComponent<Renderer>().material = isCurrentPixelOverLeftPixel ? pixelArray[pixelCount].pixelMaterial : pixelArray[pixelCount - 1].pixelMaterial;
        }
    }

    void createBottomWall()
    {
        //Creation
        pixelArray[pixelCount].bottomWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
        pixelArray[pixelCount].bottomWall.name = "bottomWall_" + pixelCount;

        //Position&Scale&Rotation
        pixelArray[pixelCount].bottomWall.transform.position = pixelArray[pixelCount].pixel.transform.position + new Vector3(0, invertFactor * (-pixelArray[pixelCount].pixelHeight / 2), -offset / 2f);
        pixelArray[pixelCount].bottomWall.transform.localScale = new Vector3(pixelWidth, 1, pixelArray[pixelCount].pixelHeight / 10f);
        pixelArray[pixelCount].bottomWall.transform.Rotate(new Vector3(90, 180, 0), Space.World);

        //Color?
        if (!whiteWall)
        {
            pixelArray[pixelCount].bottomWall.GetComponent<Renderer>().material = pixelArray[pixelCount].pixelMaterial;
        }
    }

    void createRightWall()
    {
        //Creation
        pixelArray[pixelCount - 1].rightWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
        pixelArray[pixelCount - 1].rightWall.name = "rightWall_" + pixelCount;

        //Position&Scale&Rotation
        pixelArray[pixelCount - 1].rightWall.transform.position = pixelArray[pixelCount - 1].pixel.transform.position + new Vector3(offset / 2f, invertFactor * (-pixelArray[pixelCount - 1].pixelHeight / 2), 0);
        pixelArray[pixelCount - 1].rightWall.transform.localScale = new Vector3(pixelWidth, 1, pixelArray[pixelCount - 1].pixelHeight / 10f);
        pixelArray[pixelCount - 1].rightWall.transform.Rotate(new Vector3(90, 180, 90), Space.World);

        //Color?
        if (!whiteWall)
        {
            pixelArray[pixelCount - 1].rightWall.GetComponent<Renderer>().material = pixelArray[pixelCount - 1].pixelMaterial;
        }
    }
    int getTopPixelIndex(int currentIndex)
    {
        return currentIndex - (int)imgSize.x;
    }
}
