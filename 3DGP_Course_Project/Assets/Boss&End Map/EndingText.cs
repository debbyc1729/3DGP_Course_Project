using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingText : MonoBehaviour
{
    public bool isRoll = true;
    public GameObject WinMenu;
    float originPosY;
    // Start is called before the first frame update
    void Start()
    {
        originPosY = transform.GetComponent<RectTransform>().anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.gameObject.activeSelf == true)
        {
            //Debug.Log("isRoll");
            float posX = transform.GetComponent<RectTransform>().anchoredPosition.x;
            float posY = transform.GetComponent<RectTransform>().anchoredPosition.y;
            posY += 1;
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);


            if (posY > -originPosY)
            {
                WinMenu.SetActive(true);
                isRoll = false;
            }
        }
    }
}
