using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public LayersManager.LayerType LayerType;
    public Texture2D texture;
    public bool visible = true;
    public float Opacity = 1f;
    public Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        initTexture(ref texture, name + "Texture");
        if (LayerType == LayersManager.LayerType.HeightMap) Opacity = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initTexture(ref Texture2D texture, string name)
    {
        //Init texture
        texture = new Texture2D(LayersManager.texturesSize.x, LayersManager.texturesSize.y, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None)
        {
            name = name,
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };
        var filePath = System.IO.Path.Combine(LayersManager.pathToTempTexturesFolder, texture.name + ".png");
        fillTexture();
        System.IO.File.WriteAllBytes(filePath, texture.EncodeToPNG());

        sendTextureToShader();
        sendOpacityToShader();
    }

    public void invertLayerVisibility()
    {
        visible = !visible;
        if (LayerType == LayersManager.LayerType.HeightMap && visible == false)
        {
            GetComponentInParent<LayersManager>().modeManager.setCurrentMode(ModeManager.Mode.color);
        }
        else if (LayerType == LayersManager.LayerType.HeightMap && visible == true)
        {
            GetComponentInParent<LayersManager>().modeManager.setCurrentMode(ModeManager.Mode.height);
        }
        sendOpacityToShader();
    }

    public void setLayerSliderVisibility(float value)
    {
        Opacity = value;
        sendOpacityToShader();
    }
    public void sendTextureToShader()
    {
        float trueOpacity = visible ? Opacity : 0f;
        switch (LayerType)
        {
            case LayersManager.LayerType.Background:
                GetComponentInParent<LayersManager>().drawingMesh.GetComponent<MeshRenderer>().material.SetTexture("_BackgroundTexture", texture);
                break;
            case LayersManager.LayerType.ColorMap:
                GetComponentInParent<LayersManager>().drawingMesh.GetComponent<MeshRenderer>().material.SetTexture("_ColorTexture", texture);
                break;
            case LayersManager.LayerType.HeightMap:
                GetComponentInParent<LayersManager>().drawingMesh.GetComponent<MeshRenderer>().material.SetTexture("_HeightTexture", texture);
                break;
            default: return;
        }
    }
    public void sendOpacityToShader()
    {
        float trueOpacity = visible ? Opacity : 0f;
        switch (LayerType)
        {
            case LayersManager.LayerType.Background:
                GetComponentInParent<LayersManager>().drawingMesh.GetComponent<MeshRenderer>().material.SetFloat("_BackgroundTextureAlpha", trueOpacity);
                break;
            case LayersManager.LayerType.ColorMap:
                GetComponentInParent<LayersManager>().drawingMesh.GetComponent<MeshRenderer>().material.SetFloat("_ColorTextureAlpha", trueOpacity);
                break;
            case LayersManager.LayerType.HeightMap:
                GetComponentInParent<LayersManager>().drawingMesh.GetComponent<MeshRenderer>().material.SetFloat("_HeightTextureAlpha", trueOpacity);
                break;
            default: return;
        }
    }

    public void setDefaultBackgroundColor()
    {
        GetComponent<UnityEngine.UI.Image>().color = defaultColor;
    }

    public void setSelectedBackgroundColor()
    {
        GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }

    public void fillTexture()
    {
        switch (LayerType)
        {
            case LayersManager.LayerType.Background:
                LayersManager.fillTextureWithColor(texture, Color.white); break;
            case LayersManager.LayerType.ColorMap:
                LayersManager.fillTextureWithColor(texture, new Color(0, 0, 0, 0)); break;
            case LayersManager.LayerType.HeightMap:
                LayersManager.fillTextureWithColor(texture, new Color(0.5f, 0.5f, 0.5f, 1)); break;
            default: return;
        }
    }

    public void setVisible(bool visibility)
    {
        visible = visibility;
        GetComponentInChildren<LayerVisibilityButton>().setSpriteWithVisibility(visibility);
        sendOpacityToShader();
    }
}
