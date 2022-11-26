using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogues")]

public class DialogueObject : ScriptableObject
{
    public bool hasActivated; 
    public bool onlyActivateOnce = true; 
    public Dialogue dialogue; 

}
