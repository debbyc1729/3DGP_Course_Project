using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public AudioMixer audioMixer;
    
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("More than one Game Manager");
            Destroy(this);
        }
    }
    
    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void setBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }
}
