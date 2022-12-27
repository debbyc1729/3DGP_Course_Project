using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject PasuedMenu;
    public GameObject Options;
    public GameObject WarningQuit;
    public GameObject WarningRestart;
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
                Time.timeScale = 0;
                //PasuedMenuFlg = false;
            }
            else
            {
                PasuedMenu.SetActive(false);
                Options.SetActive(false);
                WarningQuit.SetActive(false);
                WarningRestart.SetActive(false);
                Time.timeScale = 1;
                //PasuedMenuFlg = true;
            }
        }
    }
}
