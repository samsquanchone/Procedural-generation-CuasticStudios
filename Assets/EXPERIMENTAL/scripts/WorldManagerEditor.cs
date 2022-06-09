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
            manager.RaycastOnGrid();
        }
    }
}
