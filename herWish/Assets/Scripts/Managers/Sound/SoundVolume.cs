using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This makes it so this class can show up in the inspector
[System.Serializable]
public class SoundVolume
{
    public string name;

    [Range(0f, 1f)] //Determines range of volume
    public float volume;
}
