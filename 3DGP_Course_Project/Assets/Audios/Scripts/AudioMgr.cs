using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AudioMgr : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.sourceList = new List<AudioSource>();
        }
    }

    void NewSource(Sound s)
    {
        s.sourceList.Add(gameObject.AddComponent<AudioSource>());
        var audioSource = s.sourceList.Last();
        audioSource.clip = s.clip;
        audioSource.volume = s.volume;
        audioSource.pitch = s.pitch;
        audioSource.loop = s.loop;
    }

    void RemoveSource(Sound s)
    {
        if (s.sourceList.Count < 1) return;
        s.sourceList[0].Stop();
        Destroy(s.sourceList[0], 0f);
        s.sourceList.RemoveAt(0);
    }

    public void Play(string name, float duration = 0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
    
        NewSource(s);
        s.sourceList.Last().Play();

        if (!s.loop)
        {
            if (duration == 0f) duration = s.clip.length;
            StartCoroutine(FadeOut(s, duration));
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        RemoveSource(s);
    }

    IEnumerator FadeOut(Sound s, float duration)
    {
        var audioSource = s.sourceList.Last();
        float volume = audioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volume, 0f, timer / duration);
            yield return null;
        }

        // s.sourceList.Stop();
        // s.sourceList.volume = volume;
        RemoveSource(s);
        yield break;
    }
}
