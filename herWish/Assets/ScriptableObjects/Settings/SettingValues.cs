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

    [Header("FINISHED VARIABLES")]
    [SerializeField]
    private bool hasFinishedGameOnce;

    [SerializeField]
    private int timesCompleted;

    [SerializeField]
    private float[] allTimerValues;



    public void ResetTimer()
    {
        Debug.Log("Reset timer!"); 
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



    public void SetFinishedGameState(bool state)
    {
        hasFinishedGameOnce = state;
    }

    public void AddNewTime(float time)
    {
        float previousTime = 0; 

        for (int i = 0; i < allTimerValues.Length; i++) 
        { 
            if (allTimerValues[i] <= 0.1f)
            {
                Debug.Log("Adding new time"); 
                previousTime = allTimerValues[i];
                allTimerValues[i] = time;
                return;
            }

            else if (time < allTimerValues[i])
            {
                previousTime = allTimerValues[i];
                allTimerValues[i] = time;
                AddNewTime(previousTime);
                return; 
            }
        }

    }

    public void AddCompletion()
    {
        timesCompleted++;
    }

    public bool GetFinishedGameState()
    {
        return hasFinishedGameOnce; 
    }

    public int GetTimesCompleted()
    {
        return timesCompleted; 
    }

    public float GetTimerValueAtPosition(int position)
    {
        return allTimerValues[position];
    }

    public int GetAllTimerLength()
    {
        return allTimerValues.Length;
    }

    public void ResetToDefault()
    {
        SetFinishedGameState(false);

        timesCompleted = 0; 

        for (int i = 0; i < allTimerValues.Length; i++)
        {
            allTimerValues[i] = 0;
        }
    }

}
