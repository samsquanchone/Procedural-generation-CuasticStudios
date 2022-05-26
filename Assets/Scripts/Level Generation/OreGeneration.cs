using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGeneration : MonoBehaviour
{
 [SerializeField]
 private NoiseMapGeneration noiseMapGeneration;

 [SerializeField]
 private Wave[] waves;

 [SerializeField]
 private float levelScale;

 [SerializeField]
 private float[] neighborRadius; //Make array if want array of oreobjects

 [SerializeField]
 private GameObject[] orePrefab;
    
   public void GenerateOre(int levelDepth, int levelWidth, float distanceBetweenVertices, LevelData levelData)
   {
   
   //Generate oreMap using Perlin noise
   float[,] oreMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (levelDepth, levelWidth, this.levelScale, 0, 0, this.waves);
   
    float levelSizeX = levelWidth * distanceBetweenVertices;
    float levelSizeZ = levelDepth * distanceBetweenVertices;
    
    for (int zIndex = 0; zIndex < levelDepth; zIndex++) {
    for (int xIndex = 0; xIndex < levelWidth; xIndex++) {
        // convert from Level Coordinate System to Tile Coordinate System and retrieve the corresponding TileData
        TileCoordinate tileCoordinate = levelData.ConvertToTileCoordinate (zIndex, xIndex);
        TileData tileData = levelData.tilesData [tileCoordinate.tileZIndex, tileCoordinate.tileXIndex];
        int tileWidth = tileData.heightMap.GetLength (1);

        // calculate the mesh vertex index
        Vector3[] meshVertices = tileData.mesh.vertices;
        int vertexIndex = tileCoordinate.coordinateZIndex * tileWidth + tileCoordinate.coordinateXIndex;

        // get the terrain type of this coordinate
        TerrainType terrainType = tileData.chosenHeightTerrainTypes [tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];

        // get the biome of this coordinate
        Biome biome = tileData.chosenBiomes[tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];

        // check if it is a water terrain. Trees cannot be placed over the water
        if (terrainType.name != "water") {
            float oreValue = oreMap [zIndex, xIndex];

            int terrainTypeIndex = terrainType.index;

            // compares the current tree noise value to the neighbor ones
            int neighborZBegin = (int)Mathf.Max (0, zIndex - this.neighborRadius[biome.index]);
            int neighborZEnd = (int)Mathf.Min (levelDepth-1, zIndex + this.neighborRadius[biome.index]);
            int neighborXBegin = (int)Mathf.Max (0, xIndex - this.neighborRadius[biome.index]);
            int neighborXEnd = (int)Mathf.Min (levelWidth-1, xIndex + this.neighborRadius[biome.index]);
            float maxValue = 0f;
            for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++) {
                for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++) {
                    float neighborValue = oreMap [neighborZ, neighborX];
                    // saves the maximum tree noise value in the radius
                    if (neighborValue >= maxValue) {
                        maxValue = neighborValue;
                    }
                }
            }
            
                        // if the current tree noise value is the maximum one, place a tree in this location
                        if (oreValue == maxValue) {
                            Vector3 orePosition = new Vector3(xIndex*distanceBetweenVertices, meshVertices[vertexIndex].y, zIndex*distanceBetweenVertices);
                            GameObject ore = Instantiate (this.orePrefab[biome.index], orePosition, Quaternion.identity) as GameObject;
                            //ore.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);
                        }
                    }
                }
            }


   }
    
    
    
}
