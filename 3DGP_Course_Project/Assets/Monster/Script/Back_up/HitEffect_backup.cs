using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect_backup : MonoBehaviour
{
    float duration;
    void Start()
    {
        //Debug.Log("DustOnGround!!");
        duration = 0.5f;
        Destroy(gameObject, duration);
    }
}
