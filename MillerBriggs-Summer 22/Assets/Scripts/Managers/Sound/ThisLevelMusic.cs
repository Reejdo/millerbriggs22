using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisLevelMusic : MonoBehaviour
{
    public AudioManager myAudioManager;
    public AudioSource thisLevelTheme;
    public float timeToSwitch;

    public bool interactToSwitch; 

    // Start is called before the first frame update
    
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
    }


    void SwitchAudioNull()
    {
        Debug.Log("Switch Audio Null");
        AudioSource myCurrentTheme = myAudioManager.currentTheme.GetComponent<AudioSource>();
        myCurrentTheme.clip = thisLevelTheme.clip;
        myAudioManager.themeName = thisLevelTheme.clip.name;
        myCurrentTheme.loop = true;
        myAudioManager.FadeInFromSwitch();
    }

    void SwitchAudio()
    {
        StartCoroutine(myAudioManager.SwitchBackground(thisLevelTheme, timeToSwitch)); 
    }


    public void PlayerInteractAudio()
    {
        if (myAudioManager.themeName == "null")
        {
            SwitchAudioNull();
        }
        else if (myAudioManager.themeName != thisLevelTheme.clip.name && myAudioManager.themeName != null)
        {
            SwitchAudio();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !interactToSwitch)
        {
            Debug.Log("change background: " + thisLevelTheme.clip.name); 
            PlayerInteractAudio(); 
        }
    }

}
