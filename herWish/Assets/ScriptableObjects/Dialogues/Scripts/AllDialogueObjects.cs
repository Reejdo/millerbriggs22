using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllDialogues", menuName = "AllDialogueObjects")]
public class AllDialogueObjects : ScriptableObject
{
    public bool[] hasActivated;

    
    public void SetHasActivated(int dialogueNumber, bool state)
    {
        hasActivated[dialogueNumber] = state;
    }

    public void ResetAllDialogues()
    {
        for (int i = 0; i < hasActivated.Length; i++)
        {
            hasActivated[i] = false; 
        }
    }
}
