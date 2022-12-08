using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager_backup : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager_backup instance;
    A_Star_Pathfinding pathfinding;
    //static Vector3 pathEndTemp;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<A_Star_Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        //Debug.Log("RequestPath");
        //Debug.Log("pathEnd= " + pathEnd);
        //if (pathEndTemp != pathEnd)// && pathEndTemp != null
        //{
            //Debug.Log("pathRequestQueue.Clear()");
            //instance.pathRequestQueue.Clear();

            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        //}
        //pathEndTemp = pathEnd;
    }

    void TryProcessNext()
    {
        //Debug.Log("TryProcessNext");
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        //Debug.Log("FinishedProcessingPath");
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }

    }
}