using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    string audioName;
    float duration;

    // Start is called before the first frame update
    void Start()
    {
        audioName = "Boom";
        FindObjectOfType<AudioMgr>().Play(audioName);
        duration = 2f;
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
