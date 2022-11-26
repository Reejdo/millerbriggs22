using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSlidersAtStart : MonoBehaviour
{
    public Slider musicSlider, vfxSlider, voiceLineSlider;
    public SettingValues myAudioValues; 

    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = myAudioValues.masterBackgroundVolume;
        vfxSlider.value = myAudioValues.masterVFXVolume;
        voiceLineSlider.value = myAudioValues.masterVoiceLineVolume; 
    }

    public void AtStartSetSliders()
    {
        musicSlider.value = myAudioValues.masterBackgroundVolume;
        vfxSlider.value = myAudioValues.masterVFXVolume;
        voiceLineSlider.value = myAudioValues.masterVoiceLineVolume;
    }

}
