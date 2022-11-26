using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxLogCollect : MonoBehaviour
{
    public LogInventory myLogInventory;
    public AudioManager myAudioManager;
    public Sounds logSound;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            myLogInventory.SetMaxLogs();

            //play collected sound 
            myAudioManager.Play(logSound, true);

            gameObject.SetActive(false);
        }
    }
}
