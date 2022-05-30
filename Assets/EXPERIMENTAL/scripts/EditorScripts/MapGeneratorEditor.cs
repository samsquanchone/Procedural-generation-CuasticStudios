using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI() // This is all stuff for the inspector
    {
        MapGenerator generator = (MapGenerator)target; // getting a reference to the MapGenerator class

        if(DrawDefaultInspector()) // Checking if bool autoUpdate is checked
        {
            if(generator.autoUpdate)
            {
                generator.DrawMapInEditor(); // if so call gerate map
            }
        }
        
        if (GUILayout.Button("Generate")) // Making a button in the inspector, and checking if it is pressed
        {

            generator.DrawMapInEditor(); // if so call generate map
        }
    }
}
