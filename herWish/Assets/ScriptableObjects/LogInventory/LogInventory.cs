using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogInventory", menuName = "LogInventory")]

public class LogInventory : ScriptableObject
{
    [SerializeField] private int maxLogs; 
    [SerializeField] private int totalLogsCollected;
    public bool[] logsCollectedState; 


    public int GetMaxLogs()
    {
        return maxLogs; 
    }

    public int GetLogsCollected()
    {
        return totalLogsCollected; 
    }

    public void CollectLog()
    {
        totalLogsCollected++; 
    }

    public void SetMaxLogs()
    {
        totalLogsCollected = maxLogs; 
    }

    public void ResetToDefault()
    {
        totalLogsCollected = 0; 

        for (int i = 0; i < logsCollectedState.Length; i++)
        {
            logsCollectedState[i] = false; 
        }
    }

}
