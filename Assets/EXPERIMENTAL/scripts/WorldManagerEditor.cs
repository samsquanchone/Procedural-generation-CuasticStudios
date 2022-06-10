using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldManager))]
public class WorldManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldManager manager = (WorldManager)target;
        if (GUILayout.Button("Generate Trees"))
        {
            manager.GenerateTrees();
        }
        if (GUILayout.Button("Spawn Player @ x:0, z:0"))
        {
            manager.SpawnPlayer();
        }
    }
}
