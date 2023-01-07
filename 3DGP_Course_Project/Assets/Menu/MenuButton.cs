using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuButton : MonoBehaviour
{
    public GameObject Options;
    public GameObject PasuedMenu;
    public GameObject DieMenu;
    public GameObject WarningQuit;
    public GameObject WarningRestart;
    public GameObject HowToPlayMenu;
    public GameObject FadeOut;
    public Sound[] sounds = null;
    AudioMixer audioMixer;

    private void Start()
    {
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeIn();

        audioMixer = GameManager.instance.audioMixer[1];
        AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups("Master");
        //Debug.Log("audioMixGroup= " + audioMixGroup[0].name);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixGroup[0];
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PasuedMenu.activeSelf == false)
            {
                soundPlay("Menu");
                PasuedMenu.SetActive(true);
                Time.timeScale = 0;
                //PasuedMenuFlg = false;
            }
            else
            {
                soundPlay("Menu");
                PasuedMenu.SetActive(false);
                Options.SetActive(false);
                WarningQuit.SetActive(false);
                WarningRestart.SetActive(false);
                Time.timeScale = 1;
                //PasuedMenuFlg = true;
            }
        }
    }

    public void ShowStory()
    {
        soundPlay("Play");
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("StoryScene");
    }

    public void OpenHowToPlay()
    {
        soundPlay("OtherBotton");
        HowToPlayMenu.SetActive(true);
    }
    public void CloseHowToPlay()
    {
        soundPlay("OtherBotton");
        HowToPlayMenu.SetActive(false);
    }

    public void Play()
    {
        soundPlay("Play");
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("MainGameScene");
    }
    public void Resume()
    {
        soundPlay("Menu");
        PasuedMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Option()
    {
        soundPlay("OtherBotton");
        Options.SetActive(true);
    }
    public void BackFromOptions()
    {
        soundPlay("OtherBotton");
        Options.SetActive(false);
    }
    public void TryAgain()
    {
        soundPlay("Play");
        //TryAgain
        GameObject.Find("Canvas/PlayerInfo").transform.GetComponent<PlayerInfoMgr>().BackToStartPoint();
        DieMenu.SetActive(false);
        PasuedMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        //game restart (reload scene)
        soundPlay("Play");
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("MainGameScene");
    }
    public void BackToTitle()
    {
        soundPlay("Play");
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("StartMenu");
    }
    public void SureToQuit()
    {
        soundPlay("OtherBotton");
        WarningQuit.SetActive(true);
    }
    public void NOQuit()
    {
        soundPlay("OtherBotton");
        WarningQuit.SetActive(false);
    }
    public void SureToRestart()
    {
        soundPlay("OtherBotton");
        WarningRestart.SetActive(true);
    }
    public void NORestart()
    {
        soundPlay("OtherBotton");
        WarningRestart.SetActive(false);
    }
    public void Quit()
    {
/*#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif*/
        soundPlay("Play");
        FadeOut.SetActive(true);
        FadeOut.GetComponent<FadeInOut>().StartFadeOut("Quit");
    }

    public void soundPlay(string name)
    {
        //Debug.Log("soundPlay, " + name);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
