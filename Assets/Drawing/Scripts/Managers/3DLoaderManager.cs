using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Loader3DManager : MonoBehaviour
{
    public Shader defaultShader;
    private class Voxel
    {
        public GameObject cube;
        public float height;
    }

    private List<Voxel> voxels;
    public float areaWidth = 1f;
    public GameObject Object3D;
    public SaveManager SaveManager;

    private void Start()
    {
        voxels = new List<Voxel>();
        defaultShader = Shader.Find("Universal Render Pipeline/Lit");
    }

    public void load3DModel()
    {
        foreach (Voxel voxel in voxels)
        {
            Destroy(voxel.cube);
        }
        voxels.Clear();

        SaveManager.save();
        Texture2D colorTexture = SaveManager.finalColorTexture;
        Texture2D heightTexture = SaveManager.finalHeightTexture;

        Vector2 imgSize = new Vector2(colorTexture.width, colorTexture.height);
        Debug.Log(imgSize.x + "/" + imgSize.y);
        int maxImgSize = Mathf.Max((int)imgSize.x, (int)imgSize.y);
        float ratio = maxImgSize / areaWidth;
        Vector2 areaDimension = imgSize / ratio;
        float pixelWidth = areaWidth / maxImgSize;
        float offset = pixelWidth;

        for (int y = 0; y < colorTexture.height; y++)
        {
            for (int x = 0; x < colorTexture.width; x++)
            {
                voxels.Add(new Voxel());
                voxels.Last().cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                voxels.Last().cube.layer = LayerMask.NameToLayer("3DObject");
                voxels.Last().cube.name = "voxel" + x + "/" + y;
                voxels.Last().cube.transform.SetParent(Object3D.transform);
                voxels.Last().cube.transform.position = Object3D.transform.position;
                voxels.Last().cube.transform.Translate(new Vector3(-1f * (areaDimension.x / 2f) + (x + 0.5f) * offset, (-areaDimension.y / 2f) + (y + 0.5f) * offset), 0);
                voxels.Last().height = heightTexture.GetPixel(x, y).grayscale == 0 ? 0.001f : heightTexture.GetPixel(x, y).grayscale * 0.4f;
                voxels.Last().cube.transform.localScale = new Vector3(pixelWidth, pixelWidth, voxels.Last().height);
                voxels.Last().cube.transform.Translate(new Vector3(0, 0, -voxels.Last().height / 2));

                voxels.Last().cube.GetComponent<Renderer>().material = new Material(defaultShader)
                {
                    color = colorTexture.GetPixel(x, y)
                };

                if (colorTexture.GetPixel(x, y).a == 0)
                {
                    voxels.Last().cube.SetActive(false);
                }
            }
        }
        Debug.Log(voxels.Count());
    }
}
