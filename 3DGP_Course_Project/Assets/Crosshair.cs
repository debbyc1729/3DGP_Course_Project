using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;

    [HideInInspector]
    public bool hitState;
    [HideInInspector]
    public bool hitGem;
    [HideInInspector]
    public bool hitMonster;
    [HideInInspector]
    public GameObject gem;
    [HideInInspector]
    public GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        hitState = true;
        hitGem = false;
        hitMonster = false;
        gem = null;
        monster = null;
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(transform.position);
        
        if (Input.GetKey(KeyCode.T) && hitState && Physics.Raycast(ray, out hit)) {
            if (hit.transform.tag == "gem")
            {
                hitState = false;
                hitGem = true;
                gem = hit.transform.gameObject;
            }
            if (hit.transform.tag == "monster")
            {
                hitState = false;
                hitMonster = true;
                monster = hit.transform.gameObject;
            }
            if (!hitState)
            {
                StartCoroutine(DelayForHitObject());
            }
        }
    }

    IEnumerator DelayForHitObject()
    {
        yield return new WaitForSeconds(0.2f);
        hitState = true;
        hitGem = false;
        hitMonster = false;
    }
}
