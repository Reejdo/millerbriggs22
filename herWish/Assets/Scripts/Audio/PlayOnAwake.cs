using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnAwake : MonoBehaviour
{
    public AudioSource currentTheme;
    public AudioManager myAudioManager;
    private bool hasPlayed;

    [SerializeField]
    private float timeToWait = 1f; 
    private float timer; 

    void Start()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (myAudioManager == null)
        {
            myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        if (myAudioManager != null && !hasPlayed)
        {
            timer += Time.deltaTime;

            if (timer >= timeToWait)
            {
                hasPlayed = true;
                SwitchAudio();
            }
        }

    }


    void SwitchAudio()
    {
        StartCoroutine(myAudioManager.SwitchBackground(currentTheme, 0f));
    }
}
