using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBotton : MonoBehaviour
{
    public GameObject Options;
    public GameObject PasuedMenu;
    public void Play()
    {

    }
    public void Resume()
    {
        PasuedMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Option()
    {
        Options.SetActive(true);
    }
    public void BackFromOptions()
    {
        Options.SetActive(false);
    }
    public void TryAgain()
    {

    }
    public void BackToTitle()
    {

    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
