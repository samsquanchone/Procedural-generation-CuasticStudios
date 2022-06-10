using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // Player spawning stuff
    [SerializeField] GameObject playerObj;
    [SerializeField] int playerSpawnX = 0;
    [SerializeField] int playerSpawnz = 0;

    // Tree generator stuff
    [SerializeField] bool generateInEditor = false;
    [SerializeField] GameObject treeParent;
    [SerializeField] GameObject[] treePrefabs;

    // Raycast Grid size -- Only used for tree generator so far
    [SerializeField] int size = 20;
    [SerializeField] int incremant = 5;
    [SerializeField] float ySpawnOffset = 5f;

    // Satic height for the world manager object *origin for raycast so kinda important
    static int yHeight = 500;

    private void Start()
    {
        SpawnPlayer();
        if (!generateInEditor) GenerateTrees();
    }

    public void GenerateTrees()
    {     
        
        for (int i = 0, z = 0; z <= size; z = z + incremant) // Basics for raycasting on a grid
        {
            for (int x = 0; x <= size; x = x + incremant)
            {
                transform.position = new(x, yHeight, z);
                RayCastFromPoint();
                i++;
            }
        }
    }

    public void SpawnPlayer()
    {
        transform.position = new(playerSpawnX, yHeight, playerSpawnz);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1000f);
        if (hasHit)
        {
            Vector3 spawnPoint = new(hit.point.x, hit.point.y, hit.point.z);
            playerObj.transform.position = spawnPoint;
            print("Player Spawned @ " + spawnPoint.ToString());
        }
        else
        {
            Debug.Log("Did not Hit");
        }
    }

    private void RayCastFromPoint()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1000f);
        if (hasHit)
        {
            int treeToUse = Random.Range(0, treePrefabs.Length);

            // Adding 'noise' to the position
            float xNoise = Random.Range(-1, 1);
            float zNoise = Random.Range(-1, 1);
            float xPostion = hit.point.x + xNoise;
            float zPostion = hit.point.z + zNoise;

            // Adding 'noise' to scale
            float scaleNoise = Random.Range(1, 1.8f);
            treePrefabs[treeToUse].transform.localScale = new(scaleNoise, scaleNoise, scaleNoise);

            Vector3 spawnPoint = new Vector3(xPostion, (hit.point.y + ySpawnOffset), zPostion);
            Instantiate(treePrefabs[treeToUse], spawnPoint, transform.rotation, treeParent.transform);
        }
        else
        {
            Debug.Log("Did not Hit");
        }
    }
}
