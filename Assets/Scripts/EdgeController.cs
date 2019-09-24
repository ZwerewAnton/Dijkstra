using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeController : MonoBehaviour
{
    Graph<GameObject, GameObject> graph;

    void Start()
    {
        graph = GameObject.Find("Graph").GetComponent<GraphController>().graph;
    }

    void OnDestroy()
    {
        var indexRemoveEdge = graph.Edges.FindIndex(r => r.Line == gameObject);
        if(indexRemoveEdge != -1)
        {
            graph.Edges.RemoveAt(indexRemoveEdge);
        }
    }
}
