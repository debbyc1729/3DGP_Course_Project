using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    //public Transform Audio;
    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5.0f)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter= " + collision.gameObject.tag);
        /*if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
        }*/
        //Instantiate(hitEffect, collision.transform.position, transform.rotation);
        if (collision.gameObject != transform.parent.gameObject)
        {
            GameObject newHitEffect;
            newHitEffect = Instantiate(hitEffect, collision.GetContact(0).point, transform.rotation);
            //newHitEffect.transform.SetParent(transform);
            //Audio.Play(hitEffect.name);
            FindObjectOfType<MonsterAttaclAudio>().Play(transform.name);
            Destroy(transform.gameObject);
        }
    }
}
