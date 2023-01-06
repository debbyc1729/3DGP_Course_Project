using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    //public int level;
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Update()
    {

    }

    public void Play(string name)
    {
        Sound s = null;
        switch (name)
        {
            case "1":
                s = Array.Find(sounds, sound => sound.name == "Level 1");
                break;
            case "2":
                s = Array.Find(sounds, sound => sound.name == "Level 2");
                break;
            case "3":
                s = Array.Find(sounds, sound => sound.name == "Level Boss");
                break;
            default:
                Debug.Log("Play default");
                break;
        }
        Debug.Log("Play s= " + s.name);
        s.source.Play();
    }
}
