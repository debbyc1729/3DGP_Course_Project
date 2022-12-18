using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterAttaclAudio : MonoBehaviour
{
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

    public void Play(string name)
    {
        Sound s = null;
        switch (name[0])
        {
            case 'F':
                s = Array.Find(sounds, sound => sound.name == "Fire");
                break;
            case 'I':
                s = Array.Find(sounds, sound => sound.name == "Ice");
                break;
            case 'R':
                s = Array.Find(sounds, sound => sound.name == "Rock");
                break;
            default:
                break;
        }
        s.source.Play();
    }
}
