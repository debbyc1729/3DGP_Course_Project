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

        //monster slow
        if (other.gameObject.tag == "monster")
        {
            other.gameObject.GetComponent<Monster>().setMonsterSpeed(0.3f);
        }
        if (other.gameObject.tag == "boss")
        {
            other.gameObject.GetComponent<Boss>().setBossSpeed(0.3f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        script = other.gameObject.GetComponent<Player>();

        if (script != null)
        {
            script.setWalkSpeedFactor(1f);
        }

        //monster and boss slow
        if (other.gameObject.tag == "monster")
        {
            other.gameObject.GetComponent<Monster>().setMonsterSpeed(1.0f);
        }
        if (other.gameObject.tag == "boss")
        {
            other.gameObject.GetComponent<Boss>().setBossSpeed(1.0f);
        }
    }
}
