using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCollider : MonoBehaviour
{
    public bool generateNavMeshAtBuildTime = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshCollider>();

        if(generateNavMeshAtBuildTime) gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
