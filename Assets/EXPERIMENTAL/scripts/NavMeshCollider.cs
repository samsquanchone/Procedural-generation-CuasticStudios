using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCollider : MonoBehaviour
{
    public bool genNavMeshRuntime = false;

    void Start()
    {
        //gameObject.AddComponent<MeshCollider>();

        if(genNavMeshRuntime) gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

    }
}
