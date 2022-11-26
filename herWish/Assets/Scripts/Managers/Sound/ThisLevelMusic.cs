using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisLevelMusic : MonoBehaviour
{
    public AudioManager myAudioManager;
    public AudioSource thisLevelTheme;
    public float timeToSwitch;

    public bool interactToSwitch;

    private float waitTime = 0.5f, timer;
    private bool entryWaitDone;
    private bool playerIsInTrigger, hasChangedAudio; 

    // Start is called before the first frame update
    
    void Start()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); 
    }

    private void Update()
    {
        if (timer <= waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            entryWaitDone = true;
        }

        if (myAudioManager == null)
        {
            myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
    }


    void SwitchAudioNull()
    {
        //Debug.Log("Switch Audio Null");
        myAudioManager.PlayFromNull(thisLevelTheme, timeToSwitch); 
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
            playerIsInTrigger = true;

            //Debug.Log("change background: " + thisLevelTheme.clip.name); 
            if (entryWaitDone)
            {
                PlayerInteractAudio();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !interactToSwitch)
        {
            //Debug.Log("change background: " + thisLevelTheme.clip.name); 
            if (entryWaitDone && !hasChangedAudio && playerIsInTrigger)
            {
                hasChangedAudio = true; 
                PlayerInteractAudio();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !interactToSwitch)
        {
            hasChangedAudio = false;
        }
    }

}
