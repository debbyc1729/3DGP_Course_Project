using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PortalMgr : MonoBehaviour
{
    [SerializeField] GameObject portalPrefab;
    int portalCount;
    GameObject target;
    GameObject client;
    bool enableDelivering;
    string audioName;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePortals();

        target = null;
        client = null;
        enableDelivering = true;
        audioName = "Deliver";
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GeneratePortals()
    {
        portalCount = 1;

        Vector3[] portalPosList = new Vector3[] {
            new Vector3(0f, 0.2f, 0.8f), 
        };
        Color[] portalColorList = new Color[] {
            new Color(0f, 0.886f, 1f, 1f), 
            new Color(1f, 0.843f, 0f, 1f), 
            new Color(0.874f, 0f, 1f, 1f)
        };

        for (int i = 0; i < portalCount; i++)
        {
            GameObject portal = Instantiate(portalPrefab, portalPosList[i], Quaternion.identity);
            portal.transform.parent = transform;
            portal.GetComponent<Portal>().StartColor = portalColorList[i];
        }
    }

    public void OnChildTriggerEnter(int portalID, GameObject other)
    {
        if (enableDelivering)
        {
            enableDelivering = false;
            int next = (portalID + 1) % portalCount;
            target = transform.GetChild(next).gameObject;
            client = other;

            Player player = client.GetComponent<Player>();
            if (player != null)
            {
                StartCoroutine(DeliverPlayer(player));
            }
        }
    }

    public void OnChildTriggerExit(int portalID)
    {
        if (portalID == target.transform.GetSiblingIndex())
        {
            enableDelivering = true;
        }
    }

    IEnumerator DeliverPlayer(Player player)
    {
        FindObjectOfType<AudioMgr>().Play(audioName);
    
        yield return new WaitForSeconds(0.2f);
        player.ActivateMoving(false);

        yield return new WaitForSeconds(0.6f);
        client.transform.position = new Vector3(target.transform.position.x,
                                                client.transform.position.y,
                                                target.transform.position.z);
        yield return new WaitForSeconds(0.2f);
        player.ActivateMoving(true);

        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("TempScene", LoadSceneMode.Single);
    }
}
