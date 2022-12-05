using System;
using UnityEngine;

[System.Serializable]


public class FirstTimeSave
{
    private bool fts = false; 


    //return state of first time save
    public bool GetFTS()
    {
        return fts; 
    }

    public void SetFTS(bool state)
    {
        fts = state; 
    }
}
