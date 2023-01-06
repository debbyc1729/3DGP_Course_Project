using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalController : MonoBehaviour
{
    [SerializeField] GameObject portalPrefab;
    int portalCount;
    GameObject target;
    GameObject client;
    bool enableDelivering;
    string audioName;
    int respawnPortalID;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePortals();

        target = null;
        client = null;
        enableDelivering = true;
        audioName = "Deliver";
        respawnPortalID = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GeneratePortals()
    {
        portalCount = 2;

        Vector3[] portalPosList = new Vector3[] {
            new Vector3(0f, 0.1f, 1f), 
            new Vector3(-1000f, -0.29f, -1.15f), 
            // new Vector3(0f, 0.2f, 5f), 
            // new Vector3(-50f, 0.2f, -28f), 
            // new Vector3(27f, 0.2f, -40f)
        };
        Color[] portalColorList = new Color[] {
            new Color(0f, 0.886f, 1f, 1f), 
            new Color(0f, 0.886f, 1f, 1f), 
            // new Color(1f, 0.843f, 0f, 1f), 
            // new Color(0.874f, 0f, 1f, 1f)
        };

        for (int i = 0; i < portalCount; i++)
        {
            GameObject portal = Instantiate(portalPrefab, portalPosList[i], Quaternion.identity);
            portal.transform.parent = transform;
            portal.GetComponent<Portal>().StartColor = portalColorList[i];
            portal.SetActive(false);
        }
    }

    public void ShowPortals()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void HidePortals()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
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
        if (portalID == portalCount - 1)
        {
            HidePortals();
        }
    }

    public void SetPosition(GameObject obj)
    {
        Transform portal = transform.GetChild(respawnPortalID);
        obj.transform.position = new Vector3(portal.position.x,
                                             obj.transform.position.y + portal.position.y,
                                             portal.position.z);

        obj.transform.rotation = Quaternion.identity;
    }

    IEnumerator DeliverPlayer(Player player)
    {
        FindObjectOfType<AudioMgr>().Play(audioName, 3f);
    
        yield return new WaitForSeconds(0.2f);
        player.ActivateMoving(false);

        yield return new WaitForSeconds(0.6f);
        client.transform.position = new Vector3(target.transform.position.x,
                                                client.transform.position.y + target.transform.position.y,
                                                target.transform.position.z);

        client.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(0.2f);
        player.ActivateMoving(true);
    }
    
    public void SetRespawnPortalID(int i)
    {
        respawnPortalID = i;
    }
}
