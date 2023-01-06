using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public Transform EndingText;
    public GameObject Door;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter End Scene");
        if(other.tag == "Player")
        {
            transform.GetComponent<AudioSource>().Play();

            Debug.Log("Game The End.");
            Door.SetActive(true);
            EndingText.gameObject.SetActive(true);
            EndingText.GetChild(0).gameObject.SetActive(true);
            EndingText.GetChild(0).GetComponent<FadeInOut>().StartFadeOut("Ending");
        }
    }
}
