using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] bool generateInEditor = false;

    [SerializeField] GameObject treeParent;
    [SerializeField] GameObject tree;

    [SerializeField] int xSize = 20;
    [SerializeField] int zSize = 20;
    [SerializeField] int incremant = 5;
    [SerializeField] float ySpawnOffset = 5f;
    int yHeight = 500;

    private void Start()
    {
        if (!generateInEditor) RaycastOnGrid();
    }

    public void RaycastOnGrid()
    {     
        
        for (int i = 0, z = 0; z <= zSize; z = z + incremant)
        {
            for (int x = 0; x <= xSize; x = x + incremant)
            {
                transform.position = new Vector3(x, yHeight, z);
                RayCastFromPoint();
                i++;
            }
        }



        /*Verticies = new Vector3[] // Make one cell
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };*/
    }
    private void RayCastFromPoint()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1000f);
        if (hasHit)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
            Debug.Log("Did Hit");
            Vector3 spawnPoint = new Vector3(hit.point.x, (hit.point.y + ySpawnOffset), hit.point.z);
            Instantiate(tree, spawnPoint, transform.rotation, treeParent.transform);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.red);
            Debug.Log("Did not Hit");
        }
    }
}
