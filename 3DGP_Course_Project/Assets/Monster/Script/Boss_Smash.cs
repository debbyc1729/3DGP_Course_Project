using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Smash : MonoBehaviour
{
    public GameObject SmashEffect;
    public GameObject RockRain;
    public GameObject parent;
    public int maxRockNumber = 50;
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

            genRockRain();
        }
    }

    void genRockRain()
    {
        //transform.position;

        for(int i = 0; i < maxRockNumber; i++)
        {
            float rangeX = UnityEngine.Random.Range(transform.position.x - 7, transform.position.x + 7);
            float rangeZ = UnityEngine.Random.Range(transform.position.z - 7, transform.position.z + 7);

            GameObject rockRain = Instantiate(RockRain, new Vector3(rangeX, transform.position.y + 15, rangeZ), Quaternion.identity);

            //rockRain.transform.Rotate();
            //Rigidbody rockRainRb;
            //rockRainRb = rockRain.GetComponent<Rigidbody>();
            //rockRainRb.AddForce(transform.up * -4.0f, ForceMode.Impulse);

            rockRain.transform.SetParent(parent.transform);
            //SoundPlay("Throw");
        }

    }
}
