using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    float duration;
    void Start()
    {
        duration = 0.5f;
        Destroy(gameObject, duration);
    }
}
