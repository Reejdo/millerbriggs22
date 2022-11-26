using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMenuObjectState : MonoBehaviour
{
    public GameObject menuObjectOn, menuObjectOff;
    private AudioManager myAudioManager;

    private void Start()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void SetObjectState()
    {
        //play click sound
        myAudioManager.Play(Sounds.s_buttonClickUI, true);

        menuObjectOn.SetActive(true);
        menuObjectOff.SetActive(false); 
    }
}
