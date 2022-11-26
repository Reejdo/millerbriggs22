using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnWake : MonoBehaviour
{
    private AudioManager myAudioManager;
    public Sounds[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        foreach (Sounds sound in sounds)
        {
            myAudioManager.Play(sound, false); 
        }
    }
}
