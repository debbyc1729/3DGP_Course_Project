using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill
{
    public string name;
    public Image IconImage;
    [Range(0f, 1f)]
    public float damage;
    [Range(0f, 1f)]
    public float cost;
    public float duration;
    public float cooldown;
    public bool isDirectional;
    public int enableLevel;
    public PS ps;

    [HideInInspector]
    public bool flgCooldown;
    [HideInInspector]
    public Coroutine effect;
    [HideInInspector]
    public bool enable;
    [HideInInspector]
    public bool isLockedByLevel;
}

[System.Serializable]
public struct PS {
    public GameObject[] prefab;
    public Vector3 offset;
    public float lifeTime;
};
