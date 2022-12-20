using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[System.Serializable]
public class Buff
{
    public enum Type {
        Single, Multiple
    }
    public string name;
    public Type type;
    public Sprite sourceImage;
    public Color imageColor;
    public Color barColor;

    [HideInInspector]
    public BuffObj[] objList;

    public void AddObj(BuffObj newItem)
    {
        BuffObj[] destination = new BuffObj[objList.Length + 1];

        for (int i = 0; i < objList.Length; i++)
        {
            destination[i] = objList[i];
        }

        destination[objList.Length] = newItem;
        objList = destination;
    }

    public void RemoveObj(BuffObj obj)
    {
        BuffObj[] destination = new BuffObj[objList.Length - 1];
        int j = 0;

        for (int i = 0; i < objList.Length; i++)
        {
            if (objList[i].id == obj.id)
            {
                continue;
            }
            destination[j] = objList[i];
            j++;
        }

        objList = destination;
    }

    public int IndexOf(BuffObj target)
    {
        BuffObj obj = Array.Find(objList, obj => obj.id == target.id);
        return obj.index;
    }
}

[System.Serializable]
public struct BuffObj
{
    public Guid id;
    public GameObject prefab;
    public int index;
    public Vector2 position;
    public float duration;
    public Coroutine countdown;
}