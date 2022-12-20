using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    string audioName;
    float duration;

    // Start is called before the first frame update
    void Start()
    {
        audioName = "Burn";
        FindObjectOfType<AudioMgr>().Play(audioName);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Destroy()
    {
        Destroy(gameObject, 0f);
    }

    void OnDestroy()
    {
        FindObjectOfType<AudioMgr>().Stop(audioName);
    }

    public void SetPosition(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        transform.Translate(0.2f, -0.2f, 1.2f);
    }
}
