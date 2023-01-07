using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class MonsterAttaclAudio : MonoBehaviour
{
    public Sound[] sounds;
    AudioMixer audioMixer;

    void Awake()
    {
        audioMixer = GameManager.instance.audioMixer[1];
        AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups("Master/Monster");
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
                if (name[1] == 'o')
                {
                    s = Array.Find(sounds, sound => sound.name == "Rock");
                }
                if (name[1] == 'a')
                {
                    s = Array.Find(sounds, sound => sound.name == "RockRain");
                }
                break;
            case 'h':
                s = Array.Find(sounds, sound => sound.name == "hitPoint");
                break;
            case 'S':
                s = Array.Find(sounds, sound => sound.name == "Smash");
                break;
            case 'B':
                s = Array.Find(sounds, sound => sound.name == "BossDie");
                break;
            case 'T':
                s = Array.Find(sounds, sound => sound.name == "Tornado");
                break;
            default:
                break;
        }
        s.source.Play();
    }
}
