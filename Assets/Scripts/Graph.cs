using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph<TVertexType, TEdgeType>
{
    public List<Vertex<TVertexType>> Vertices { get; private set; }
    public List<Edge<TVertexType, TEdgeType>> Edges { get; private set; }

    public Graph()
    {
        Vertices = new List<Vertex<TVertexType>>();
        Edges = new List<Edge<TVertexType, TEdgeType>>();
    }
}

public class Vertex<TVertexType>
{
    public TVertexType Object { get; set; }
}

public class Edge<TVertexType, TEdgeType>
{
    public float Weight { get; set; }
    public TEdgeType Line { get; set; }
    public Vertex<TVertexType> Vertex1 { get; set; }
    public Vertex<TVertexType> Vertex2 { get; set; }
}

public class Dijkstra<TVertexType, TEdgeType>
{
    public static List<Vertex<TVertexType>> DijkstraAlgoritm(Graph<TVertexType, TEdgeType> graph, Vertex<TVertexType> start, Vertex<TVertexType> end)
    {
        var notVisited =  new List<Vertex<TVertexType>>();
        notVisited.AddRange(graph.Vertices);

        var edges = graph.Edges;
        var myEdges = new List<Edge<TVertexType, TEdgeType>>();

        var path = new Dictionary<Vertex<TVertexType>, DijkstraData<TVertexType>>();
        path[start] = new DijkstraData<TVertexType> { Weight = default, Previous = default };

        while (true)
        {
            Vertex<TVertexType> toOpen = null;
            float bestPrice = float.PositiveInfinity;
            foreach (var e in notVisited)
            {
                if (path.ContainsKey(e) && path[e].Weight < bestPrice)
                {
                    bestPrice = path[e].Weight;
                    toOpen = e;
                }
            }

            if (toOpen == default) return null;
            if (toOpen == end) break;

            for(int i = 0; i < edges.Count; i++)
            {
                if(edges[i].Vertex1 == toOpen)
                {
                    var currentPrice = path[toOpen].Weight + edges[i].Weight;
                    var nextNode = edges[i].Vertex2;
                    if (!path.ContainsKey(nextNode) || path[nextNode].Weight > currentPrice)
                    {
                        path[nextNode] = new DijkstraData<TVertexType> { Previous = toOpen, Weight = currentPrice };
                    }
                }
                if(edges[i].Vertex2 == toOpen)
                {
                    var currentPrice = path[toOpen].Weight + edges[i].Weight;
                    var nextNode = edges[i].Vertex1;
                    if (!path.ContainsKey(nextNode) || path[nextNode].Weight > currentPrice)
                    {
                        path[nextNode] = new DijkstraData<TVertexType> { Previous = toOpen, Weight = currentPrice };
                    }
                }
            }
            notVisited.Remove(toOpen);
        }

        var result = new List<Vertex<TVertexType>>();
        while (end != null)
        {
            result.Add(end);
            end = path[end].Previous;
        }
        result.Reverse();
        return result;
    }
}

class DijkstraData<TVertexType>
{
    public Vertex<TVertexType> Previous { get; set; }
    public float Weight { get; set; }
}


