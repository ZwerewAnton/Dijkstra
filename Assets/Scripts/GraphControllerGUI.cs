using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GraphController))]
public class GraphControllerGUI : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GraphController g_controller = (GraphController)target;
        if(GUILayout.Button("Find shortest path"))
        {
            g_controller.Pathfinder();
        }
        if (GUILayout.Button("Add vertex"))
        {
            g_controller.AddVertex();
        }
        if (GUILayout.Button("Add edge"))
        {
            g_controller.AddEdge();
        }
    }
}
