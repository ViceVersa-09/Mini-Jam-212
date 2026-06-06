using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] Vector2 gridOffset = new Vector2(0.5f, 0.5f);
    [SerializeField] Vector2 minBounds;
    [SerializeField] Vector2 maxBounds;
    [SerializeField] GameObject NodePrefab;
    [Header("Testing Setttings")]
    [SerializeField] Node testStart;
    [SerializeField] Node testEnd;
    [SerializeField] bool test;
    public List<Node> result;

    public Node player;

    public static Pathfinding Instance;
    List<Node> allNodes;

    private void Awake()
    {
        Instance = this;
        allNodes = new List<Node>(((int)maxBounds.x - (int)minBounds.x) * ((int)maxBounds.y - (int)minBounds.y));
        CreateGrid();
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            result = GeneratePath(testStart, testEnd);
        }
    }

    public List<Node> GeneratePath(Node start, Node end) // This Will Generate A Path To The End Node From The Start Node By Checking The Connections Of The Start Node And Assigning Values Based On Distance To End Node And Then It Repeats For Every Nodes Connections. It Will Add The Closest Node To The List And Skip The Rest. If The Closest Connection Is The End Node Then It Will Finish The Loop.
    {
        List<Node> openSet = new List<Node>();

        foreach (Node n in allNodes)
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(end.transform.position, start.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if (currentNode == end)
            {
                List<Node> path = new List<Node>();

                path.Insert(0, end);

                while (currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }
                path.Reverse();

                return path;
            }

            foreach (Node connectedNode in currentNode.connections)
            {
                if (connectedNode != null && connectedNode.nodeEnabled)
                {
                    float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                    if (heldGScore < connectedNode.gScore)
                    {
                        connectedNode.cameFrom = currentNode;
                        connectedNode.gScore = heldGScore;
                        connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);
                        
                        if (!openSet.Contains(connectedNode))
                        {
                            openSet.Add(connectedNode);
                        }
                    }
                }
            }   
        }

        return null;
    }

    void CreateGrid()
    {
        for (float y = minBounds.y; y < maxBounds.y; y++)
        {
            for (float x = minBounds.x; x < maxBounds.x; x++)
            {
                allNodes.Add(Instantiate(NodePrefab, new Vector2(x, y) + gridOffset, Quaternion.identity, transform).GetComponent<Node>());
            }
        }
    }

    public List<Node> GetConnections(Node current)
    {
        List<Node> connections = new List<Node>();

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 || y == 0) 
                {
                    Vector2 connectionPosition = (Vector2)current.transform.position + new Vector2(x, y);

                    foreach (Node node in allNodes)
                    {
                        if ((Vector2)node.transform.position == connectionPosition && connectionPosition != (Vector2)current.transform.position)
                        {
                            connections.Add(node);
                        }
                    }
                }
            }
        }
        return connections;
    }
}
