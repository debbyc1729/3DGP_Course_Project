using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    [HideInInspector]
    public string name;
    Player script;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy() 
    {
        if (script != null)
        {
            script.setWalkSpeedFactor(1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        script = other.gameObject.GetComponent<Player>();

        if (script != null)
        {
            script.setWalkSpeedFactor(0.3f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        script = other.gameObject.GetComponent<Player>();

        if (script != null)
        {
            script.setWalkSpeedFactor(1f);
        }
    }
}
