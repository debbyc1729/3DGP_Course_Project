using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    Vector3 maxScale;
    float duration;

    public Color StartColor
    {
        get { return GetComponent<ParticleSystem>().main.startColor.color; }
        set {
            ParticleSystem[] childrenPS = GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem childPS in childrenPS)
            {
                var main = childPS.main;
                main.startColor = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        maxScale = Vector3.one * 2f;
        duration = 1f;
        StartCoroutine(ScaleAnimation());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ScaleAnimation()
    {
        float timer = 0f;
        float halfDuration = duration / 2f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, maxScale, timer / halfDuration);
            yield return null;
        }
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, maxScale, timer / halfDuration);
            yield return null;
        }
        Destroy(gameObject, 0f);
    }
}
