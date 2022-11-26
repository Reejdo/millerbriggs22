using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//If we want to be able to edit this class in the inspector, need serializable
[System.Serializable]
public class Dialogue
{
    //public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    public AudioClip[] audioClips; 
    //public Sprite[] portraits;
}
