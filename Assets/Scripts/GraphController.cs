using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GraphController : MonoBehaviour
{
    public GameObject vertexPrefab, linePrefab;
    public Graph<GameObject, GameObject> graph;
    public Material blackLine;
    public GameObject from = null, to = null;
    GameObject linesContainer = null, verticiesContainer = null;

    float[,] arrayGraphStart = { { -4, 0, 4, 0 }, 
                                 { 0, 4, 0, -4 } };

    void Start()
    {
        graph = new Graph<GameObject, GameObject>();
        linesContainer = GameObject.FindGameObjectWithTag("Lines");
        verticiesContainer = GameObject.FindGameObjectWithTag("Verticies");

        for (int i = 0; i < 4; i++)
        {
            var newVertex = Instantiate(vertexPrefab, new Vector2(arrayGraphStart[0, i], arrayGraphStart[1, i]), Quaternion.identity);
            newVertex.transform.parent = verticiesContainer.transform;

            var vertex = new Vertex<GameObject>() { Object = newVertex };
            graph.Vertices.Add(vertex);
        }

        for(int i = 0; i < graph.Vertices.Count; i++)
        {
            var newLine = Instantiate(linePrefab);
            newLine.transform.parent = linesContainer.transform;

            var lineRenderer = newLine.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.positionCount = 2;
            lineRenderer.material = blackLine;

            if (i == graph.Vertices.Count - 1)
            {
                var edge = new Edge<GameObject, GameObject>()
                {
                    Line = newLine,
                    Weight = Vector2.Distance(graph.Vertices[i].Object.transform.position, graph.Vertices[0].Object.transform.position),
                    Vertex1 = graph.Vertices[i],
                    Vertex2 = graph.Vertices[0]
                };
                graph.Edges.Add(edge);
            }
            else
            {
                var edge = new Edge<GameObject, GameObject>()
                {
                    Line = newLine,
                    Weight = Vector2.Distance(graph.Vertices[i].Object.transform.position, graph.Vertices[i + 1].Object.transform.position),
                    Vertex1 = graph.Vertices[i],
                    Vertex2 = graph.Vertices[i + 1]
                };
                graph.Edges.Add(edge);
            }
        }
    }

    void Update()
    {
        foreach (var v in graph.Edges)
        {
            v.Line.GetComponent<LineRenderer>().SetPosition(0, v.Vertex1.Object.transform.position);
            v.Line.GetComponent<LineRenderer>().SetPosition(1, v.Vertex2.Object.transform.position);
        }
    }
    public void Pathfinder()
    {
        if ((from != null && to != null) || from.tag != "Vertex" || to.tag != "Vertex")
        {
            foreach (var edge in graph.Edges)
            {
                edge.Weight = Vector2.Distance(edge.Vertex1.Object.transform.position, edge.Vertex2.Object.transform.position);
            }

            List<Vertex<GameObject>> listVerticies = Dijkstra<GameObject, GameObject>.DijkstraAlgoritm(graph, graph.Vertices[graph.Vertices.IndexOf(graph.Vertices.Find(i => i.Object == from))],
                                                                                    graph.Vertices[graph.Vertices.IndexOf(graph.Vertices.Find(i => i.Object == to))]);
            if(listVerticies != null)
            {
                foreach (var allVert in graph.Vertices)
                {
                    allVert.Object.GetComponent<SpriteRenderer>().color = Color.white;
                    foreach (var bestVert in listVerticies)
                    {
                        if(bestVert == allVert)
                        {
                            bestVert.Object.GetComponent<SpriteRenderer>().color = Color.green;
                        }
                    }
                }
            }
        }
    }

    public void AddVertex()
    {
        var newVertex = Instantiate(vertexPrefab);
        newVertex.transform.parent = verticiesContainer.transform;

        var vertex = new Vertex<GameObject>() { Object = newVertex };
        graph.Vertices.Add(vertex);
    }

    public void AddEdge()
    {
        foreach (var v in graph.Edges)
        {
            if((v.Vertex1.Object == from && v.Vertex2.Object == to) || (v.Vertex2.Object == from && v.Vertex1.Object == to))
            {
                return;
            }
        }

        if ((from != null && to != null) || from.tag != "Vertex" || to.tag != "Vertex" || from == to)
        {
            var newLine = Instantiate(linePrefab);
            newLine.transform.parent = linesContainer.transform;

            var lineRenderer = newLine.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.positionCount = 2;
            lineRenderer.material = blackLine;

            var indexFrom = graph.Vertices.FindIndex(r => r.Object == from);
            var indexTo = graph.Vertices.FindIndex(r => r.Object == to);
            var edge = new Edge<GameObject, GameObject>()
                {
                    Line = newLine,
                    Weight = Vector2.Distance(graph.Vertices[indexFrom].Object.transform.position, graph.Vertices[indexTo].Object.transform.position),
                    Vertex1 = graph.Vertices[indexFrom],
                    Vertex2 = graph.Vertices[indexTo]
                };
            graph.Edges.Add(edge);
        }
    }
}
