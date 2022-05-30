using System.Collections;
using UnityEngine;

public static class Noise 
{
    public enum NormaliseMode {  Local, Global };

   public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormaliseMode normaliseMode)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed); // pusedo random number generator
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0; ;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offestX = prng.Next(-100000, 100000) + offset.x;
            float offestY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offestX, offestY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        // Checking sclae isnt 0, dont wanna be divding by that lol
        #region 
        if (scale <= 0)
        {
            scale = 0.0001f;
        }
        #endregion

        float localMaxNoiseHeight = float.MinValue;
        float localMinNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2;
        float halfHeight = mapHeight / 2;

        for (int y = 0; y < mapHeight; y++ )
        {
            for ( int x = 0; x < mapWidth; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale  * frequency ;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency ;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > localMaxNoiseHeight) // Keeping track of the highest noise height
                {
                    localMaxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < localMinNoiseHeight) // keeping track of lowest noise height
                {
                    localMinNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (normaliseMode == NormaliseMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(localMinNoiseHeight, localMaxNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    float normalisedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight);
                    noiseMap[x, y] = Mathf.Clamp(normalisedHeight, 0, int.MaxValue);
                }
            }
        }

        return noiseMap;
    }
}
