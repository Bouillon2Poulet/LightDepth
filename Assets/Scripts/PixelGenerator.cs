using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGenerator : MonoBehaviour
{
    public Texture2D image;
    Vector2 imageDimension;
    Vector2 areaDimension;
    float areaWidth = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int imgSize  = GetTextureDimensions.GetDimensions(Application.dataPath+"/Textures/"+image.name+".png");
        imageDimension.x = imgSize.x;
        imageDimension.y = imgSize.y;
        Debug.Log(imageDimension);

        int maxImageDimension = Mathf.Max((int)imageDimension.x, (int)imageDimension.y);
        Debug.Log(maxImageDimension);

        
        float ratio = maxImageDimension/areaWidth;
        areaDimension = imageDimension/ratio;
        float pixelWidth = areaWidth/maxImageDimension;
        float offset = pixelWidth*10f;
        
        GameObject[] pixels = new GameObject[(int)imageDimension.x * (int)imageDimension.y];
        GameObject[] UP_WALL = new GameObject[(int)imageDimension.x * (int)imageDimension.y];
        GameObject[] DOWN_WALL = new GameObject[(int)imageDimension.x];
        GameObject[] LEFT_WALL = new GameObject[(int)imageDimension.x * (int)imageDimension.y];
        GameObject[] RIGHT_WALL = new GameObject[(int)imageDimension.y];


        int pixelCount = 0;
        for(int i=0; i<imageDimension.y; i++)
        {
            for(int j=0; j<imageDimension.x; j++)
            {
                //PIXEL
                pixels[pixelCount] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                pixels[pixelCount].transform.parent = this.gameObject.transform;

                Debug.Log("Creating pixel x="+(j+1).ToString()+" y="+(i+1).ToString());
                
                float height = image.GetPixel(j,(int)imageDimension.y-i-1).grayscale; //Start at bottom left

                Debug.Log(((areaDimension.y*10/2f)-(i+0.5f)*offset).ToString());
                int horizontalFactor = 1;
                pixels[pixelCount].transform.position = new Vector3(-1f*(areaDimension.x*10/2f)+(j+0.5f)*offset,height,(areaDimension.y*10/2f)-(i+0.5f)*offset);
                pixels[pixelCount].transform.localScale = new Vector3(pixelWidth,1,pixelWidth);

                Material pixelMaterial = new Material(Shader.Find("Custom/DoubleSidedShader"));
                pixelMaterial.color = image.GetPixel(j,(int)imageDimension.y-i-1);
                pixels[pixelCount].GetComponent<Renderer>().material = pixelMaterial;

                //UP WALL
                UP_WALL[pixelCount] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                float upwardPixelYPosition = (pixelCount-imageDimension.x<0) ? 0 : pixels[(int)(pixelCount-imageDimension.x)].transform.position.y;
                float upWallHeight = Mathf.Abs(pixels[pixelCount].transform.position.y - upwardPixelYPosition);
                bool isCurrentPixelOverUpwardPixel = (pixels[pixelCount].transform.position.y > upwardPixelYPosition) ? true : false;
                float movingFactorUp = isCurrentPixelOverUpwardPixel? -1f : 1f;
                UP_WALL[pixelCount].transform.position = pixels[pixelCount].transform.position + new Vector3(0f,movingFactorUp*upWallHeight/2f,offset/2f);
                UP_WALL[pixelCount].transform.localScale = new Vector3(pixelWidth,1,upWallHeight/10f);
                float YRotationUp = (!isCurrentPixelOverUpwardPixel)? 180f : 0f;
                UP_WALL[pixelCount].transform.Rotate(new Vector3(90,YRotationUp,0),Space.World);

                //LEFT WALL
                LEFT_WALL[pixelCount] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                float leftPixelYPosition = (pixelCount%imageDimension.x==0) ? 0 : pixels[(int)(pixelCount-1)].transform.position.y;
                float leftWallHeight = Mathf.Abs(pixels[pixelCount].transform.position.y - leftPixelYPosition);
                bool isCurrentPixelOverLeftPixel = (pixels[pixelCount].transform.position.y > leftPixelYPosition) ? true : false;
                float movingFactorLeft = isCurrentPixelOverLeftPixel? -1f : 1f;
                LEFT_WALL[pixelCount].transform.position = pixels[pixelCount].transform.position + new Vector3(-offset/2f,movingFactorLeft*leftWallHeight/2f,0);
                LEFT_WALL[pixelCount].transform.localScale = new Vector3(pixelWidth,1,leftWallHeight/10f);
                float YRotationLeft = (!isCurrentPixelOverLeftPixel)? 180f : 0f;
                LEFT_WALL[pixelCount].transform.Rotate(new Vector3(90,YRotationLeft,90),Space.World);

                if(i==imageDimension.y-1)
                {
                    DOWN_WALL[j] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    DOWN_WALL[j].transform.position = pixels[pixelCount].transform.position + new Vector3(0,-pixels[pixelCount].transform.position.y/2,-offset/2f);
                    DOWN_WALL[j].transform.localScale = new Vector3(pixelWidth,1,pixels[pixelCount].transform.position.y/10f);
                    DOWN_WALL[j].transform.Rotate(new Vector3(90,180,0),Space.World);
                }
                pixelCount++;
            }
            //RIGHT WALL (ONLY FOR RIGHT SIDE)
            RIGHT_WALL[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            RIGHT_WALL[i].transform.position = pixels[pixelCount-1].transform.position + new Vector3(offset/2f,-pixels[pixelCount-1].transform.position.y/2,0);
            RIGHT_WALL[i].transform.localScale = new Vector3(pixelWidth,1,pixels[pixelCount-1].transform.position.y/10f);
            RIGHT_WALL[i].transform.Rotate(new Vector3(90,180,90),Space.World);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
