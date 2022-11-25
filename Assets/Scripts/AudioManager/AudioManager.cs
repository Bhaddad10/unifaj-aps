using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

// Manages playing/stopping Audio in an easy way
public class AudioManager : MonoBehaviour
{
    // Editor saves the sounds to be used
    public Sound[] sounds;

    // Singleton
    public static AudioManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
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

    // Plays sound 'name'
    public void Play(string name)
    {
        PlayDelayed(name);
    }

    // Plays sound 'name' after a delay of 'delay' seconds
    public void PlayDelayed(string name, float delay = 0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: '" + name + "' not found.");
            return;
        }

        s.source.PlayDelayed(delay);
    }

    // Stop playing sound 'name' 
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

    // Reset() allows for easier managing on UI
    private void Reset()
    {
        sounds = new Sound[]
        {
            new Sound()
        };
    }

    // Plays one of the 8 footstep sounds at random
    internal void PlayWalk()
    {
        Play("FootstepMetal0" + Random.Range(1, 9));
    }

    // Stops all sounds
    internal void StopAll()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }
}
