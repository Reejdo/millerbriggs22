using UnityEngine.Audio; //Much audio stuff is in this namespace
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

public class VoiceLineManager : MonoBehaviour
{

    public Sound voiceLine; 

    public float defaultVolume = 1f;

    public string folderLocation = ""; 

    public static VoiceLineManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        /* This would matter if it wasn't attached to Audio Manager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; //Makes sure no other code is run before destroying gameObject
        }


        DontDestroyOnLoad(gameObject);
        */ 

        voiceLine.source = gameObject.AddComponent<AudioSource>();
        voiceLine.source.clip = voiceLine.clip;

        voiceLine.source.volume = defaultVolume;
        voiceLine.source.pitch = 1f;
        voiceLine.source.loop = voiceLine.loop;
        voiceLine.source.playOnAwake = voiceLine.playOnAwake;

    }

    public void Play(AudioClip myAudioClip)
    {

        if (myAudioClip == null)
        {
            Debug.Log("Voice line: " + name + " not found!!");
        }
        else
        {
            //these two are just visual
            voiceLine.name = name;
            voiceLine.clip = myAudioClip; 

            voiceLine.source.clip = myAudioClip;
            voiceLine.source.volume = defaultVolume;
            voiceLine.source.Play();
        }
    }

    public void StopSound()
    {
        voiceLine.source.Stop(); 
    }

    public void SetVoiceLineVolume(float newVolume)
    {
        defaultVolume = newVolume;

        voiceLine.source.volume = newVolume; 
    }

}
