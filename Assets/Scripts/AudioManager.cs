using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;


    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //Play("theme");
    }



    public void PlayDelayed(string name, float delay = 0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: '" + s.name + "' not found.");
            return;
        }

        s.source.PlayDelayed(delay);
    }

    public void Play(string name)
    {
        PlayDelayed(name);
    }

    internal void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: '" + name + "' not found.");
            return;
        }

        s.source.Stop();
    }

    // For default values
    private void Reset()
    {
        sounds = new Sound[]
        {
            new Sound()
        };
    }

    internal void PlayWalk()
    {
        Play("FootstepMetal0" + Random.Range(1, 9));
    }
}
