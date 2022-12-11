using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject beamPrefab;

    PortalMgr portalMgr;
    int index;

    public Color StartColor
    {
        get { return GetComponent<ParticleSystem>().main.startColor.color; }
        set {
            var main = GetComponent<ParticleSystem>().main;
            main.startColor = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        portalMgr = FindObjectOfType<PortalMgr>();
        index = transform.GetSiblingIndex();
    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnTriggerEnter(Collider other)
    {
        EmitBeam();
        portalMgr.OnChildTriggerEnter(index, other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        portalMgr.OnChildTriggerExit(index);
    }

    void EmitBeam()
    {
        GameObject beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
        beam.transform.parent = transform;
        beam.GetComponent<Beam>().StartColor = StartColor;
    }
}
