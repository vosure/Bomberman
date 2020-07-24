using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;

        if (DrawDefaultInspector() || GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }

        //if (GUILayout.Button("Generate Map"))
        //{
        //    map.GenerateMap();
        //}
    }
}
