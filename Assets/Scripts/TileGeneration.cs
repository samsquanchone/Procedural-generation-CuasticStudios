using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable] //Function to set terrain colour, name and height within the inspector
public class TerrainType {
  public string name;
  public float height;
  public Color color;
}
*/
[System.Serializable]
public class Wave {
  public float seed;
  public float frequency;
  public float amplitude;
}
[System.Serializable]
public class Biome {
  public string name;
  public Color color;
  public int index;
}
[System.Serializable]
public class BiomeRow {
  public Biome[] biomes;
}
[System.Serializable]
public class TerrainType {
        public string name;
        public float threshold;
        public Color color;
        public int index;
}

public class TileGeneration : MonoBehaviour
{

[SerializeField]
  private BiomeRow[] biomes;


      [SerializeField]
      private TerrainType[] heightTerrainTypes;
      [SerializeField]
      private TerrainType[] heatTerrainTypes;
      
      [SerializeField] private AnimationCurve heatCurve;
      
    [SerializeField]
    private Color waterColor;
      
   
      [SerializeField]
      ProceduralGeneration noiseMapGeneration;
      [SerializeField]
      private MeshRenderer tileRenderer;
      [SerializeField]
      private MeshFilter meshFilter;
      [SerializeField]
      private MeshCollider meshCollider;
      [SerializeField]
      private float mapScale;
      
      [SerializeField]
      private float heightMultiplier;
            
      [SerializeField]
      private AnimationCurve heightCurve;
      
      [SerializeField]
      private Wave[] waves;
      
      [SerializeField]
      private VisualizationMode visualizationMode;
      
      [SerializeField]
        private TerrainType[] moistureTerrainTypes;
      [SerializeField]
        private AnimationCurve moistureCurve;
      [SerializeField]
        private Wave[] moistureWaves;
      
    
     
    
      /*void Awake() {
 // GenerateTile (10000, 10000);
      }
     */
     
      
     public TileData GenerateTile(float centerVertexZ, float maxDistanceZ) {
          // calculate tile depth and width based on the mesh vertices
          Vector3[] meshVertices = this.meshFilter.mesh.vertices;
          int tileDepth = (int)Mathf.Sqrt (meshVertices.Length);
          int tileWidth = tileDepth;
          // calculate the offsets based on the tile position
          float offsetX = -this.gameObject.transform.position.x;
          float offsetZ = -this.gameObject.transform.position.z;
          // generate a heightMap using Perlin Noise
          float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap (tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);
          // calculate vertex offset based on the Tile position and the distance between vertices
          Vector3 tileDimensions = this.meshFilter.mesh.bounds.size;
          float distanceBetweenVertices = tileDimensions.z / (float)tileDepth;
          float vertexOffsetZ = this.gameObject.transform.position.z / distanceBetweenVertices;
          // generate a heatMap using uniform noise
          float[,] uniformedHeatMap = this.noiseMapGeneration.GenerateUniformNoiseMap (tileDepth, tileWidth, centerVertexZ, maxDistanceZ, vertexOffsetZ);
          float[,] randomHeatMap = this.noiseMapGeneration.GenerateNoiseMap (tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);
          float[,] heatMap = new float[tileDepth, tileWidth];
          for (int zIndex = 0; zIndex < tileDepth; zIndex++){
          for (int xIndex = 0; xIndex < tileWidth; xIndex++){
          
          heatMap [zIndex, xIndex] = uniformedHeatMap [zIndex, xIndex] * randomHeatMap [zIndex, xIndex];
          heatMap [zIndex, xIndex] += this.heatCurve.Evaluate(heightMap [zIndex, xIndex] * heightMap [zIndex, xIndex]);
          }
          
        }
        
        // generate a moistureMap using Perlin Noise
        float[,] moistureMap = this.noiseMapGeneration.GenerateNoiseMap (tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.moistureWaves);
        for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
          for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
            // makes higher regions dryer, by reducing the height value from the heat map
            moistureMap [zIndex, xIndex] -= this.moistureCurve.Evaluate(heightMap [zIndex, xIndex]) * heightMap [zIndex, xIndex];
          }
        }
          
           // build a Texture2D from the height map
          TerrainType[,] chosenHeightTerrainTypes = new TerrainType[tileDepth, tileWidth];
          Texture2D heightTexture = BuildTexture (heightMap, this.heightTerrainTypes, chosenHeightTerrainTypes);
          
          
          // build a Texture2D from the heat map
          TerrainType[,] chosenHeatTerrainTypes = new TerrainType[tileDepth, tileWidth];
          Texture2D heatTexture = BuildTexture (heatMap, this.heatTerrainTypes, chosenHeatTerrainTypes);
          
          // build a Texture2D from the moisture map
          TerrainType[,] chosenMoistureTerrainTypes = new TerrainType[tileDepth, tileWidth];
          Texture2D moistureTexture = BuildTexture (moistureMap, this.moistureTerrainTypes, chosenMoistureTerrainTypes);
          
          // build a biomes Texture2D from the three other noise variables
          Biome[,] chosenBiomes = new Biome[tileDepth, tileWidth];
          Texture2D biomeTexture = BuildBiomeTexture(chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes, chosenBiomes);
          
          //Used for assinging material to respective visulization mode choice
          switch (this.visualizationMode) {
          case VisualizationMode.Height:
            // assign material texture to be the heightTexture
            this.tileRenderer.material.mainTexture = heightTexture;
            break;
          case VisualizationMode.Heat:
            // assign material texture to be the heatTexture
            this.tileRenderer.material.mainTexture = heatTexture;
            break;
          
          case VisualizationMode.Moisture:
               // assign material texture to be the moistureTexture
               this.tileRenderer.material.mainTexture = moistureTexture;
               break;
               
         case VisualizationMode.Biome:
           // assign material texture to be the moistureTexture
           this.tileRenderer.material.mainTexture = biomeTexture;
           break;
          }
          // update the tile mesh vertices according to the height map
                   UpdateMeshVertices (heightMap);
                   
                   TileData tileData = new TileData (heightMap, heatMap, moistureMap,
                       chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes, chosenBiomes,
                       this.meshFilter.mesh) /*(Texture2D)this.tileRenderer.material.mainTexture)*/;

                   return tileData;
                 }
               
          
           enum VisualizationMode {Height, Heat, Moisture, Biome}  //Declaring enum for visualisation selection
          
          private Texture2D BuildBiomeTexture(TerrainType[,] heightTerrainTypes, TerrainType[,] heatTerrainTypes, TerrainType[,] moistureTerrainTypes, Biome[,] chosenBiomes) {
            int tileDepth = heatTerrainTypes.GetLength (0);
            int tileWidth = heatTerrainTypes.GetLength (1);
            Color[] colorMap = new Color[tileDepth * tileWidth];
            for (int zIndex = 0; zIndex < tileDepth; zIndex++) {

              for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
              
              int colorIndex = zIndex * tileWidth + xIndex;
              TerrainType heightTerrainType = heightTerrainTypes [zIndex, xIndex];
                // check if the current coordinate is a water region
                if (heightTerrainType.name != "water") {
                  // if a coordinate is not water, its biome will be defined by the heat and moisture values
                  TerrainType heatTerrainType = heatTerrainTypes [zIndex, xIndex];
                  TerrainType moistureTerrainType = moistureTerrainTypes [zIndex, xIndex];
                  // terrain type index is used to access the biomes table
                  Biome biome = this.biomes [moistureTerrainType.index].biomes [heatTerrainType.index];
                  // assign the color according to the selected biome
                  colorMap [colorIndex] = biome.color;
                  
                  // save biome in chosenBiomes matrix only when it is not water
                  chosenBiomes [zIndex, xIndex] = biome;
                } else {
                  // water regions don't have biomes, they always have the same color
                  colorMap [colorIndex] = this.waterColor;
                }
              }
            }
            // create a new texture and set its pixel colors
            Texture2D tileTexture = new Texture2D (tileWidth, tileDepth);
            tileTexture.filterMode = FilterMode.Point;
            tileTexture.wrapMode = TextureWrapMode.Clamp;
            tileTexture.SetPixels (colorMap);
            tileTexture.Apply ();
            return tileTexture;
          }
         
      
      
     private Texture2D BuildTexture(float[,] heightMap, TerrainType[] terrainTypes, TerrainType[,] chosenTerrainTypes) {
        int tileDepth = heightMap.GetLength (0);
        int tileWidth = heightMap.GetLength (1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
          for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
            // transform the 2D map index is an Array index
            int colorIndex = zIndex * tileWidth + xIndex;
            float height = heightMap [zIndex, xIndex];
            // choose a terrain type according to the height value
            TerrainType terrainType = ChooseTerrainType (height, terrainTypes);
            // assign the color according to the terrain type
            colorMap[colorIndex] = terrainType.color;
            chosenTerrainTypes [zIndex, xIndex] = terrainType;
          }
        }
        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D (tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels (colorMap);
        tileTexture.Apply ();
        return tileTexture;
      }
    TerrainType ChooseTerrainType(float height, TerrainType[] terrainTypes) {
      // for each terrain type, check if the height is lower than the one for the terrain type
      foreach (TerrainType heightTerrainType in heightTerrainTypes) {
        // return the first terrain type whose height is higher than the generated one
        if (height < heightTerrainType.threshold) {
          return heightTerrainType;
        }
      }
      return heightTerrainTypes [heightTerrainTypes.Length - 1];
    }
    
    private void UpdateMeshVertices(float[,] heightMap) {
           
              int tileDepth = heightMap.GetLength (0);
              int tileWidth = heightMap.GetLength (1);
              Vector3[] meshVertices = this.meshFilter.mesh.vertices;
              
              // iterate through all the heightMap coordinates, updating the vertex index
              int vertexIndex = 0;
              for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
                for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
                
                  float height = heightMap [zIndex, xIndex];
                  Vector3 vertex = meshVertices [vertexIndex];
                  // change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
                  meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);
                  vertexIndex++;
                }
              }
              // update the vertices in the mesh and update its properties
              this.meshFilter.mesh.vertices = meshVertices;
              this.meshFilter.mesh.RecalculateBounds ();
              this.meshFilter.mesh.RecalculateNormals ();
              // update the mesh collider
              this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }
}

public class TileData {
  public float[,]  heightMap;
  public float[,]  heatMap;
  public float[,]  moistureMap;
  public TerrainType[,] chosenHeightTerrainTypes;
  public TerrainType[,] chosenHeatTerrainTypes;
  public TerrainType[,] chosenMoistureTerrainTypes;
  public Biome[,] chosenBiomes;
  public Mesh mesh;
 // public Texture2D texture;
  
  public TileData(float[,]  heightMap, float[,]  heatMap, float[,]  moistureMap,
    TerrainType[,] chosenHeightTerrainTypes, TerrainType[,] chosenHeatTerrainTypes, TerrainType[,] chosenMoistureTerrainTypes,
    Biome[,] chosenBiomes, Mesh mesh /*, Texture2D texture*/) {
    this.heightMap = heightMap;
    this.heatMap = heatMap;
    this.moistureMap = moistureMap;
    this.chosenHeightTerrainTypes = chosenHeightTerrainTypes;
    this.chosenHeatTerrainTypes = chosenHeatTerrainTypes;
    this.chosenMoistureTerrainTypes = chosenMoistureTerrainTypes;
    this.chosenBiomes = chosenBiomes;
    this.mesh = mesh;
  }
}
