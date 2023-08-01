using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class System_AudioManager : MonoBehaviour
{
    [SerializeField]
    public string nameToPlay { private get; set; }

    [SerializeField]
    private Object_Sound[] audioList;

    private void Awake()
    {
        foreach (Object_Sound sound in audioList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.sound;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    public void SetSoundNameToPlay(string name)
    {
        nameToPlay = name;
        PlaySound (nameToPlay);
    }

    public void PlaySound(string name)
    {
        Object_Sound sound =
            Array.Find(audioList, test => test.soundName.Equals(name));
        if (sound != null)
        {
            if (name.Equals("Parry") || name.Equals("PerfectParry"))
            {
                sound.source.pitch = sound.pitch;
                sound.source.pitch += UnityEngine.Random.Range(-0.1f, 0.1f);
            }
            else if (name.Equals("Gunshot"))
            {
                sound.source.pitch = sound.pitch;
                sound.source.pitch += UnityEngine.Random.Range(-0.2f, 0.2f);
            }
            sound.source.Play();
        }
        else
        {
            print("Cannot find sound");
        }
    }
}
