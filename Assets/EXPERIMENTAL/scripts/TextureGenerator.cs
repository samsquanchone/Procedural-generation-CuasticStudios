using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator 
{
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0); // getting the 'x', or first, dimenson of the float array
        int height = heightMap.GetLength(1); // getting the 'y',or second
        Color[] colourMap = new Color[width * height]; // One dimensoional color array with size of area 

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]); // setting each point in color map in relation to x,y of the noiseMap

            }
        }

        return TextureFromColourMap(colourMap, width, height);
    }
}
