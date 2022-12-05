using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSlidersAtStart : MonoBehaviour
{
    public Slider musicSlider, vfxSlider, voiceLineSlider;
    public SettingValues myAudioValues;

    private AudioManager myAudioManager;
    bool setSliders; 

    // Start is called before the first frame update
    void Start()
    {
        myAudioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (myAudioManager != null && !setSliders)
        {
            setSliders = true;
            AtStartSetSliders(); 
        }
        else if (myAudioManager == null)
        {
            myAudioManager = GameObject.FindObjectOfType<AudioManager>();
        }
    }


    public void AtStartSetSliders()
    {
        musicSlider.value = myAudioValues.masterBackgroundVolume;
        vfxSlider.value = myAudioValues.masterVFXVolume;
        voiceLineSlider.value = myAudioValues.masterVoiceLineVolume;
    }

}
