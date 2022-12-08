using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_backup : MonoBehaviour
{
    public bool displayGridGizmos;
    public bool displaySphereOfPath;
    public Transform player;
    public GameObject Sphere;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        Vector3 objectScale = Sphere.transform.localScale;
        Sphere.transform.localScale = new Vector3(objectScale.x * nodeRadius, objectScale.y * nodeRadius, objectScale.z * nodeRadius);

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //if (x == 0 && y == 0)
                if ((x == 0 && y == 0) || (x != 0 && y != 0))
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    /*void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }*/

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));


        if (grid != null && displayGridGizmos)
        {
            //Node playerNode = NodeFromWorldPoint(player.position);
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            Node playerNode = null;
            if(Physics.Raycast(ray, out hitData, Mathf.Infinity))
            {
                playerNode = NodeFromWorldPoint(hitData.point);
                //Debug.Log("hitData.point= " + hitData.point);
            }*/

            foreach (Node n in grid)
            {
                //Debug.Log("hitData.point= " + n.worldPosition);
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                //Debug.Log("n= " + n.worldPosition + ", playerNode= " + playerNode.worldPosition);
                /*if (playerNode == n)
                {
                    //Debug.Log("n and playerNode cover!!");
                    Gizmos.color = Color.cyan;
                }*/

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    public void GenerateSphere()
    {
        if (displaySphereOfPath)
        {
            foreach (Node n in grid)
            {
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        //Instantiate_Sphere.Generate(n.worldPosition);
                        Instantiate(Sphere, n.worldPosition, Quaternion.identity);
                        //nodeRadius
                    }
                }
            }
        }
    }
    public void ClearSphere()
    {
        if (displaySphereOfPath)
        {
            GameObject[] tagObject;
            tagObject = GameObject.FindGameObjectsWithTag("SphereOfPath");
            for (int i = 0; i < tagObject.Length; i++)
            {
                if (tagObject[i].name == "SphereOfPath(Clone)")
                {
                    Destroy(tagObject[i].gameObject);
                }
            }
        }
    }
}