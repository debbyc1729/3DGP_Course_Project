using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject PasuedMenu;
    bool PasuedMenuFlg = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PasuedMenu.activeSelf == false)
            {
                PasuedMenu.SetActive(true);
                //PasuedMenuFlg = false;
            }
            else
            {
                PasuedMenu.SetActive(false);
                //PasuedMenuFlg = true;
            }
        }
    }
}
