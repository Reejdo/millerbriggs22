using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompleteGameSettings", menuName = "FinishedSettings")]

public class CompleteGameSettings : ScriptableObject
{
    [SerializeField]
    private bool hasFinishedGameOnce;

    [SerializeField]
    private int timesCompleted; 

    [SerializeField]
    private float[] allTimerValues; 



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

}
