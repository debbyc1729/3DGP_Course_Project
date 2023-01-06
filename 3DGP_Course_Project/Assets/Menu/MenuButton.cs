using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject Options;
    public GameObject PasuedMenu;
    public GameObject WarningQuit;
    public GameObject WarningRestart;
    public GameObject FadeOut;

    private void Start()
    {
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeIn();
    }

    public void ShowStory()
    {
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("StoryScene");
    }

    public void Play()
    {
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("MainGameScene");
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
        //TryAgain
    }
    public void Restart()
    {
        //game restart (reload scene)
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("MainGameScene");
    }
    public void BackToTitle()
    {
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("StartMenu");
    }
    public void SureToQuit()
    {
        WarningQuit.SetActive(true);
    }
    public void NOQuit()
    {
        WarningQuit.SetActive(false);
    }
    public void SureToRestart()
    {
        WarningRestart.SetActive(true);
    }
    public void NORestart()
    {
        WarningRestart.SetActive(false);
    }
    public void Quit()
    {
/*#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif*/
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("Quit");
    }
}
