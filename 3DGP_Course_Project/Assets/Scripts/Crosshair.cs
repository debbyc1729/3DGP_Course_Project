using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    RaycastHit hit;

    [HideInInspector]
    public Ray ray;
    [HideInInspector]
    public bool hitGem;
    [HideInInspector]
    public GameObject gem;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        hitGem = false;
        gem = null;
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(transform.position);
        hitGem = false;
        
        if (Physics.Raycast(ray, out hit, 45, mask)) {
            if (hit.transform.tag == "gem")
            {
                hitGem = true;
                gem = hit.transform.gameObject;
            }
        }
    }
    
    void OnDrawGizmos() {
        Gizmos.color=Color.red;
        ray = Camera.main.ScreenPointToRay(transform.position);
        Gizmos.DrawRay(ray);
    }

}
