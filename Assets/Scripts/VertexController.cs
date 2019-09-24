using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexController : MonoBehaviour
{
    Graph<GameObject, GameObject> graph;

    void Start()
    {
        graph = GameObject.Find("Graph").GetComponent<GraphController>().graph;
    }

    void OnDestroy()
    {
        for (int i = graph.Edges.Count - 1; i >= 0; i--)
        {
            if ((graph.Edges[i].Vertex1.Object == gameObject) || (graph.Edges[i].Vertex2.Object == gameObject))
            {
                Destroy(graph.Edges[i].Line);
                graph.Edges.Remove(graph.Edges[i]);
            }
        }

        var indexRemoveVertex = graph.Vertices.FindIndex(r => r.Object == gameObject);
        graph.Vertices.RemoveAt(indexRemoveVertex);
    }
}
