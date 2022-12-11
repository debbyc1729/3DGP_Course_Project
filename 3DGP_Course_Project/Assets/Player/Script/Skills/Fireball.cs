using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleCollisionEvent> collisionEvents;
    string audioName;
    float duration;
    [SerializeField] GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        audioName = "Throw";
        FindObjectOfType<AudioMgr>().Play(audioName);
        duration = 3f;
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Boom");
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
        Vector3 contactPos = collisionEvents[0].intersection;
        explosionPrefab.transform.position = contactPos;
        explosionPrefab.transform.rotation = transform.rotation;
        GameObject explosion = Instantiate(explosionPrefab, explosionPrefab.transform.position, explosionPrefab.transform.rotation);
        explosion.transform.parent = transform.parent;
        Destroy(gameObject, 0f);
    }
}
