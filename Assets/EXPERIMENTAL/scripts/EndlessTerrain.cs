using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    const float scale = 15f;

    const float viewerMoveThresholdForChunkUpdate = 25f; // Distance the player need to move before chunk updates
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate; // same as above but square dude

    public LODInfo[] detailLevels;
    public static float maxViewDstance;

    public Transform viewer; // Possibly the player
    public Material mapMaterial;

    public static Vector2 viewerPositon; // Player position ??
    Vector2 viewrPosOld; 
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunkVisibleInViewDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunkVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDstance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisibleInViewDistance = Mathf.RoundToInt(maxViewDstance / chunkSize);

        UpdateVisibleChunk();
    }

    private void Update()
    {
        viewerPositon = new Vector2(viewer.position.x, viewer.position.z) / scale;

        if((viewrPosOld - viewerPositon).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewrPosOld = viewerPositon;
            UpdateVisibleChunk();
        }
    }

    void UpdateVisibleChunk()
    {
        // This is to make usre the chunks visibles last update wont be visible if still out of range form 'viewer'player
        #region
        for (int i = 0; i < terrainChunkVisibleLastUpdate.Count; i++)
        {
            terrainChunkVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunkVisibleLastUpdate.Clear();
        #endregion

        int currentChunkCoorX = Mathf.RoundToInt(viewerPositon.x / chunkSize);
        int currentChunkCoorY = Mathf.RoundToInt(viewerPositon.y / chunkSize);

        for (int yOffset = -chunkVisibleInViewDistance; yOffset <= chunkVisibleInViewDistance; yOffset++) // Looping thourhg chunk starting fomr top left corner -- if you think about it on a 3x3 grid
        {
            for (int xOffset = -chunkVisibleInViewDistance; xOffset <= chunkVisibleInViewDistance; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoorX + xOffset, currentChunkCoorY + yOffset);

                if(terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();                    
                }
                if(!terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
                }

            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MapData mapData;
        bool mapDataRecived;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;

        int previousLodIndex = -1;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material)
        {
            this.detailLevels = detailLevels;

            position = coord * size;
            Vector3 postionV3 = new Vector3(position.x, 0, position.y);
            bounds = new Bounds(position, Vector2.one * size);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;

            meshObject.transform.position = postionV3 * scale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * scale;
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }

            mapGenerator.RequestMapData(position, OnMapDataRecived);
        }

        void OnMapDataRecived(MapData mapData)
        {
            this.mapData = mapData;
            mapDataRecived = true;

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }

        void OnMeshDataRecived(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }

        public void UpdateTerrainChunk()
        {
            if(mapDataRecived)
            {
                float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPositon));
                bool visible = viewerDistanceFromNearestEdge <= maxViewDstance;

                if(visible)
                {
                    int currenLodIndex = 0;
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if(viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold)
                        {
                            currenLodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (currenLodIndex != previousLodIndex)
                    {
                        LODMesh lodMesh = lodMeshes[currenLodIndex];
                        if(lodMesh.hasMesh)
                        {
                            previousLodIndex = currenLodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                            meshCollider.sharedMesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }
                    terrainChunkVisibleLastUpdate.Add(this);
                }

                SetVisible(visible);
            }
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }

    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        void OnMeshDataRecived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallback();
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecived);
        }
    }

    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDistanceThreshold;
    }
}


