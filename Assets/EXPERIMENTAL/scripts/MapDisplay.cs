using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture) // draw noise map using 2d float array 
    {
        textureRenderer.sharedMaterial.mainTexture = texture; // adding texture to shared materials so game doesnt not need ot be rendered
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height); // making the texture renderer the same size as the noiseMap
    }
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
