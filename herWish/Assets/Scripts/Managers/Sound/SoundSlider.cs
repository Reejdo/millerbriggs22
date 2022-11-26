using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public Slider mySlider;
    public SettingValues myAudioValue;
    public AudioManager myAudioManager;
    private SaveScriptableData mySaveData; 


    private void Start()
    {
        myAudioManager = GameObject.FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        mySaveData = GameObject.FindObjectOfType<SaveScriptableData>().GetComponent<SaveScriptableData>();
    }

    private void Update()
    {
        if (mySaveData == null)
        {
            mySaveData = GameObject.FindObjectOfType<SaveScriptableData>().GetComponent<SaveScriptableData>();
        }

        if (myAudioManager == null) 
        {
            myAudioManager = GameObject.FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        }
    }


    public void UpdateMusicVolume()
    {
        myAudioValue.masterBackgroundVolume = mySlider.value;

        SetAudioValues(); 

        //Debug.Log("Save Game Data");
        
        if (mySaveData!= null) 
        {
            mySaveData.SaveGame();
        }

    }

    public void UpdateVFXVolume()
    {
        myAudioValue.masterVFXVolume = mySlider.value;

        SetAudioValues(); 

        //Debug.Log("Save Game Data");

        if (mySaveData != null)
        {
            mySaveData.SaveGame();
        }
    }

    public void UpdateVFXPlayNoise()
    {
        //play jump sound for player when changing VFX volume
        myAudioManager.Play(Sounds.s_woodCollect, true);
    }

    public void UpdateVoiceLineVolume()
    {
        myAudioValue.masterVoiceLineVolume = mySlider.value;

        SetAudioValues(); 

        //Debug.Log("Save Game Data");

        if (mySaveData != null)
        {
            mySaveData.SaveGame();
        }
    }


    private void SetAudioValues()
    {
        if (myAudioManager == null)
        {
            //Debug.Log("Can't find audio manager!");
            myAudioManager = GameObject.FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        }

        if (myAudioManager != null)
        {
            myAudioManager.SetAudioValues();
        }
    }
}
