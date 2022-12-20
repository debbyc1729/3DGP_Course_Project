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
    public float consumption;
    public float duration;
    public float cooldown;
    public bool isDirectional;
    public PS ps;

    [HideInInspector]
    public bool flgCooldown;
    public Coroutine effect;
}

[System.Serializable]
public struct PS {
    public GameObject[] prefab;
    public Vector3 offset;
    public float lifeTime;
};
