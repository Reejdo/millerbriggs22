using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    public int logNumber;
    public LogInventory myLogInventory;
    public AudioManager myAudioManager;
    public Sounds logSound; 
    private bool hasInteracted = false; 

    private void Awake()
    {
        if (myLogInventory.logsCollectedState[logNumber])
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); 
    }


    private void Update()
    {
        if (myAudioManager == null)
        {
            myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasInteracted)
        {
            hasInteracted = true; 
            myLogInventory.logsCollectedState[logNumber] = true;
            myLogInventory.CollectLog();

            //play collected sound 
            myAudioManager.Play(logSound, true); 

            gameObject.SetActive(false); 
        }
    }

}
