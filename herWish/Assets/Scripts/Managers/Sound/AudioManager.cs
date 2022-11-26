using UnityEngine.Audio; //Much audio stuff is in this namespace
using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public float soundEffectDefaultPitch = 0.3f;
    public float soundEffectPitchDeviation = 0.2f;

    //public Sound currentBackground;

    public AudioSource currentTheme;
    public string themeName;

    public static AudioManager instance;

    public float maxBackgroundVolume = 0.7f;
    public float backgroundVolume;
    public float currentBackgroundVolume = 0;
    float fade;
    float fadeSpeed, fadeTime = 1f;

    public SettingValues myAudioValues;
    public VoiceLineManager myVoiceLineManager;

    private bool switchingAudio = false;
    public CurrentThemeManager myCurrentThemeManager; 

    // Start is called before the first frame update
    void Awake()
    {
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


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }

        SetAudioValues();

        SetBckgVolumeDefault();
        UpdateCurrentTheme();

    }


    void Update()
    {
        currentBackgroundVolume = currentTheme.volume; 
    }


    public void SetAudioValues()
    {
        //Debug.Log("Setting audio values"); 

        //Set all the background values to the new background volume
        maxBackgroundVolume = myAudioValues.masterBackgroundVolume;
        backgroundVolume = maxBackgroundVolume;
        currentBackgroundVolume = maxBackgroundVolume;
        currentTheme.volume = maxBackgroundVolume; 

        myVoiceLineManager.SetVoiceLineVolume(myAudioValues.masterVoiceLineVolume);
        
        //Need to update this so it fades right away to the adjusted background val, not fading to previous background val
        myCurrentThemeManager.FadeIn(0, backgroundVolume);


        for (int i = 0; i < sounds.Length; i++)
        {
            Sounds mySound = Sounds.s_nullSound;

            if (sounds[i].name == myAudioValues.soundVolumes[i].name)
            {
                //Debug.Log("found " + sounds[i].name);

                float newVolume = myAudioValues.masterVFXVolume * myAudioValues.soundVolumes[i].volume;

                foreach (Sounds soundType in Enum.GetValues(typeof(Sounds)))
                {
                    if (sounds[i].name == soundType.ToString())
                    {
                        mySound = soundType; 
                    }
                }

                ChangeSoundVolume(mySound, newVolume);
            }
            else
            {
                Debug.Log("Cannot find " + sounds[i].name + " in Sound Volumes!");
            }
        }
    }


    public void Play(Sounds sound, bool deviatePitch)
    {
        //Debug.Log("Play sound: " + sound.ToString()); 

        string name = sound.ToString(); 

        //This uses using.System
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!!");
        }
        else
        {
            if (deviatePitch)
            {
                //Debug.Log("play " + name); 
                float highPitch = soundEffectDefaultPitch + soundEffectPitchDeviation;
                float lowPitch = soundEffectDefaultPitch - soundEffectPitchDeviation;
                float newPitch = Random.Range(lowPitch, highPitch);

                s.source.pitch = newPitch;
            }

            s.source.Play();
        }
    }

    public void ChangeSoundVolume(Sounds sound, float newVolume)
    {
        string name = sound.ToString();

        //This uses using.System
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!!");
        }
        else if (s != null)
        {
            s.source.volume = newVolume; 
        }
    }

    public void StopSound(Sounds sound)
    {
        string name = sound.ToString();

        //This uses using.System
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!!");
        }
        else if (s != null)
        {
            s.source.Stop();
        }
    }



    public void UpdateCurrentTheme()
    {
        themeName = currentTheme.clip.name;
        currentTheme.Play(); 
    }

    public void SetMaxVolumeAndPlay(float duration)
    {
        //StartCoroutine(BackgroundFade(currentTheme, 2, backgroundVolume));
        SetBackgroundVolume(maxBackgroundVolume);
        currentTheme.Play();

    }

    public void PlayFromNull(AudioSource source, float duration)
    {
        //Debug.Log("Play From Null"); 

        currentTheme.clip = source.clip;
        currentTheme.clip.name = source.clip.name;
        currentTheme.pitch = source.pitch;
        currentTheme.loop = source.loop;
        themeName = currentTheme.clip.name;

        SetMaxVolumeAndPlay(duration); 
        myCurrentThemeManager.FadeIn(duration, backgroundVolume);
    }

    public IEnumerator SwitchBackground(AudioSource source, float duration)
    {
        if (!switchingAudio)
        {
            switchingAudio = true; 

            Debug.Log("Switch Background");
            myCurrentThemeManager.FadeOut(duration, currentBackgroundVolume);

            SetBackgroundVolume(0f);

            yield return new WaitForSeconds(duration);

            currentTheme.clip = source.clip;
            currentTheme.clip.name = source.clip.name;
            currentTheme.pitch = source.pitch;
            currentTheme.loop = source.loop;

            themeName = currentTheme.clip.name;

            SetMaxVolumeAndPlay(duration);

            myCurrentThemeManager.FadeIn(duration, backgroundVolume); 
            
            switchingAudio = false; 
        }
    }


    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = volume; 
    }

    public void SetBckgVolumeDefault()
    {
        backgroundVolume = maxBackgroundVolume; 
    }

}




public enum Sounds
{
    s_nullSound, 
    s_rainLight,
    s_woodCollect,
    s_pWalk, 
    s_pRevJump, 
    s_pJump,
    s_pJumpLand, 
    s_completeLogs, 
    s_buttonClickUI
    
}
