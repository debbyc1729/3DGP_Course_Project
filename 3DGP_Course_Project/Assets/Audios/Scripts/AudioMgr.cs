using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioMgr : MonoBehaviour
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

    public void Play(string name, float duration = 0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();

        if (!s.loop)
        {
            if (duration == 0f) duration = s.clip.length;
            StartCoroutine(FadeOut(s, duration));
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    IEnumerator FadeOut(Sound s, float duration)
    {
        float volume = s.source.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            s.source.volume = Mathf.Lerp(volume, 0f, timer / duration);
            yield return null;
        }

        s.source.Stop();
        s.source.volume = volume;
        yield break;
    }
}
