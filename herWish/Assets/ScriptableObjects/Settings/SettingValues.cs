using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingValues", menuName = "Settings")]

public class SettingValues : ScriptableObject
{
    public float masterBackgroundVolume;
    public float masterVoiceLineVolume;
    public float masterVFXVolume; 
    public SoundVolume[] soundVolumes;

    public bool visualEffects = true;
    public bool fullScreen = true;
    public bool dialogueOn = true;
    public bool timerOn = false;

    [SerializeField] private float timerValue = 0.1f; 

    
    public void ResetTimer()
    {
        timerValue = 0.1f; 
    }

    public void SetTimerValue(float value)
    {
        timerValue = value;
    }

    public float GetTimerValue() 
    {
        return timerValue; 
    }
}
