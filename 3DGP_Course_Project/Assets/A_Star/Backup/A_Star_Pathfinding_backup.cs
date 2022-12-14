using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class A_Star_Pathfinding_backup : MonoBehaviour
{
    PathRequestManager requestManager;
    public Transform seeker, target;
    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Debug.Log("StartFindPath");
        StopCoroutine(FindPath(startPos, targetPos));
        StartCoroutine(FindPath(startPos, targetPos));
    }

    /*void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        //Node playerNode = null;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitData, Mathf.Infinity))
        {
            //playerNode = NodeFromWorldPoint(hitData.point);
            //Debug.Log("hitData.point= " + hitData.point);
            grid.ClearSphere();

            FindPath(seeker.position, hitData.point);
        }

        //FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //List<Node> openSet = new List<Node>();
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            /*Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
        grid.GenerateSphere();

    }*/

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Debug.Log("FindPath");

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //Debug.Log("startNode.walkable= " + startNode.walkable + ", targetNode.walkable= " + targetNode.walkable);
        //Debug.Log("targetNode.walkable= " + targetNode.walkable);

        if (targetNode.walkable)//startNode.walkable && 
        {
            //List<Node> openSet = new List<Node>();
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            //Debug.Log("openSet.Count= " + openSet.Count);
            while (openSet.Count > 0)
            {
                /*Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                    {
                        if (openSet[i].hCost < currentNode.hCost)
                            currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                */
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            grid.ClearSphere();
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        //Debug.Log("RetracePath");
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //Vector3[] waypoints = SimplifyPath(path);
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        path.Reverse();
        grid.path = path;
        grid.GenerateSphere();
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 0; i < path.Count; i++)
        {
            //Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            //if (directionNew != directionOld)
            //{
            waypoints.Add(path[i].worldPosition);
            //}
            //directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }


}