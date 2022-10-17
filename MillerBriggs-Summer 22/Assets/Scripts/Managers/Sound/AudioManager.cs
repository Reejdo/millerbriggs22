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

        SetBckgVolumeDefault(); 
        UpdateCurrentTheme();

    }


    public void Play(Sounds sound)
    {
        string name = sound.ToString(); 

        //This uses using.System
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!!");
        }
        else
        {
            //Debug.Log("play " + name); 
            float highPitch = soundEffectDefaultPitch + soundEffectPitchDeviation;
            float lowPitch = soundEffectDefaultPitch - soundEffectPitchDeviation;
            float newPitch = Random.Range(lowPitch, highPitch);

            s.source.pitch = newPitch; 
            s.source.Play();
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

    public void StopTheme()
    {
        //StartCoroutine(BackgroundFade(currentBackground.source, timeToFade, 0));
        SetBackgroundVolume(0f); 
    }

    public void UpdateCurrentTheme()
    {
        themeName = currentTheme.clip.name;
        currentTheme.Play(); 
    }


    public void FadeInFromSwitch()
    {
        //Debug.Log("Fade in from Switch"); 
        currentTheme.Play();
        //StartCoroutine(BackgroundFade(currentTheme, 2, backgroundVolume));
        SetBackgroundVolume(maxBackgroundVolume); 

    }


    public IEnumerator SwitchBackground(AudioSource source, float duration)
    {
        Debug.Log("Switch Background");

        StopTheme();

        yield return new WaitForSeconds(duration);

        currentTheme.clip = source.clip;
        currentTheme.clip.name = source.clip.name;
        currentTheme.pitch = source.pitch;
        currentTheme.loop = source.loop;

        themeName = currentTheme.clip.name;

        FadeInFromSwitch(); 

        Debug.Log("Switch Background Fade Done");
    }


    /*public static IEnumerator BackgroundFade(AudioSource audioSource, float duration, float targetVolume)
    {
        Debug.Log("Background Fade"); 
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);

            yield return null;
        }

        Debug.Log("Background Fade Done");

        yield break;
    }
    */


    // Update is called once per frame
    void Update()
    {
        currentBackgroundVolume = backgroundVolume; 

        if (currentTheme.volume != currentBackgroundVolume)
        {
            //Debug.Log("Fade in!");
            fade = Mathf.SmoothDamp(currentTheme.volume, currentBackgroundVolume, ref fadeSpeed, fadeTime);
            currentTheme.volume = fade; 
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
    Jump,
    Fall,
    f_fireBall,
    f_greenSkull,
    f_defaultEnemy,
    f_boneBullet,
    p_Hurt,
    p_Death,
    hurt_DefaultEnemy,
    hurt_BoneEnemy,
    death_DefaultEnemy,
    death_BoneEnemy,
    death_BigBoneEnemy,
    bulletWallImpact, 
    dropItem, 
    healthItem, 
    am_Skull,
    am_default, 
    gainAmmo_Skull
}
