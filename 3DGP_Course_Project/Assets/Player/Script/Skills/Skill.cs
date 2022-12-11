using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string name;
    public GameObject[] prefab;
    public float damage;
    public float consumption;
    public Vector3 offset;
    public float lifeTime;
    public float duration;
}
