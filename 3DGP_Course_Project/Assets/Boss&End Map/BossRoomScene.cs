using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomScene : MonoBehaviour
{
    public GameObject StartMountain;
    public GameObject EndMountain;
    // Start is called before the first frame update
    void Start()
    {
        //StartMountain = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("goToEndScene()");
            goToEndScene();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter Boss Room Scene");
        if (other.tag == "Player")
        {
            Debug.Log("Game Boss Fight Start.");
            StartMountain.SetActive(true);
        }
    }

    public void goToEndScene()
    {

    }
}
