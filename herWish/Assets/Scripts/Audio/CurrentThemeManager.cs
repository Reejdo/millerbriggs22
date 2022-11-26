using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentThemeManager : MonoBehaviour
{
    public AudioSource currentTheme;
    float fade;
    float fadeSpeed;
    public bool fadeIn;
    float duration;
    float maxVolume; 

    public void FadeOut(float _duration, float startVolume)
    {
        SetFadeIn(false); 
        currentTheme.volume = startVolume;
        duration = _duration; 
    }

    public void FadeIn(float _duration, float _maxVolume)
    {
        SetFadeIn(true);
        duration = _duration;
        maxVolume = _maxVolume; 
    }

    void SetFadeIn(bool state)
    {
        fadeIn = state; 
    }


    private void Update()
    {
        if (!fadeIn)
        {
            if(currentTheme.volume != 0f)
            {
                fade = Mathf.SmoothDamp(currentTheme.volume, 0f, ref fadeSpeed, duration);
                currentTheme.volume = fade;
                Debug.Log("Fading out!");
            }
        }

        if (fadeIn)
        {
            if (currentTheme.volume != maxVolume)
            {
                fade = Mathf.SmoothDamp(currentTheme.volume, maxVolume, ref fadeSpeed, duration);
                currentTheme.volume = fade;
                Debug.Log("Fading in - vol: " + maxVolume);
            }
        }
    }
}
