using UnityEngine.Audio;
using UnityEngine;

//This makes it so this class can show up in the inspector
[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] //Determines range of volume
    public float volume;
    [Range(.1f, 3f)] //Determines range of pitch
    public float pitch;

    public bool loop;
    public bool playOnAwake;

    //We populate this right away so we don't need to see it
    [HideInInspector]
    public AudioSource source;
}
