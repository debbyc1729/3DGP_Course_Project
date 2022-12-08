using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustOnGround_backup : MonoBehaviour
{
    float duration;
    void Start()
    {
        //Debug.Log("DustOnGround!!");
        duration = 0.5f;
        Destroy(gameObject, duration);
    }

    /*public void Destroy()
    {
        Destroy(gameObject, 0f);
    }*/
}
