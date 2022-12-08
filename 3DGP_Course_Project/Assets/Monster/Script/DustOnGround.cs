using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustOnGround : MonoBehaviour
{
    float duration;
    void Start()
    {
        duration = 0.5f;
        Destroy(gameObject, duration);
    }
}
