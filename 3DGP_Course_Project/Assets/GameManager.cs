using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public AudioMixer[] audioMixer;
    
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
        audioMixer[0].SetFloat("MasterVolume", volume);
    }

    public void setBGMVolume(float volume)
    {
        audioMixer[0].SetFloat("BGMVolume", volume);
    }
    public void setPlayerVolume(float volume)
    {
        audioMixer[1].SetFloat("PlayerVolume", volume);
    }

    public void setMonsterVolume(float volume)
    {
        audioMixer[1].SetFloat("MonsterVolume", volume);
    }

    public float getMasterVolume()
    {
        float volume;
        audioMixer[0].GetFloat("MasterVolume", out volume);
        return volume;
    }

    public float getBGMVolume()
    {
        float volume;
        audioMixer[0].GetFloat("BGMVolume", out volume);
        return volume;
    }
    public float getPlayerVolume()
    {
        float volume;
        audioMixer[1].GetFloat("PlayerVolume", out volume);
        return volume;
    }

    public float getMonsterVolume()
    {
        float volume;
        audioMixer[1].GetFloat("MonsterVolume", out volume);
        return volume;
    }

}
