using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Smash : MonoBehaviour
{
    public GameObject SmashEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("OnCollisionEnter");
            GameObject newHitEffect;
            newHitEffect = Instantiate(SmashEffect, collision.GetContact(0).point, transform.rotation);
            FindObjectOfType<MonsterAttaclAudio>().Play("Smash");

            //this.gameObject.SetActive(false);
            transform.GetComponent<Collider>().enabled = false;
        }
    }
}
