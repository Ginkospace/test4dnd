using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup mixer;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(-3f, 3f)]
    public float pitch = 1;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class Audio : MonoBehaviour
{   public static Audio instance;

    public Sound[] BGM, sounds;
    public int nowBGM = 0;
    
    void Awake()
    {   if (instance != null){Destroy(gameObject);return;}
        else{instance = this;DontDestroyOnLoad(gameObject);}

        foreach (Sound theSound in sounds)
        {   theSound.source = gameObject.AddComponent<AudioSource>();
            theSound.source.clip = theSound.clip;
            theSound.source.volume = theSound.volume;
            theSound.source.pitch = theSound.pitch;
            theSound.source.loop = theSound.loop;
        }

        foreach (Sound theSound in BGM)
        {
            theSound.source = gameObject.AddComponent<AudioSource>();
            theSound.source.clip = theSound.clip;
            theSound.source.volume = theSound.volume;
            theSound.source.pitch = theSound.pitch;
            theSound.source.loop = theSound.loop;
        }
    }

    public void Play(string sound)
    {   Sound theSound = Array.Find(sounds, item => item.name == sound);
        theSound.source.Play();
    }
    public void Stop(string sound)
    {   Sound theSound = Array.Find(sounds, item => item.name == sound);
        theSound.source.Stop();
    }

    public void StopBGM(){foreach (Sound theSound in BGM){ theSound.source.Stop(); }}

    public void playBGM(int next)
    {
        nowBGM += next; StopBGM();
        Sound theSound = BGM[nowBGM%BGM.Length]; 
        theSound.source.Play();
    }
}
