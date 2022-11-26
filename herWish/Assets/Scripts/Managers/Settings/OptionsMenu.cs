using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject settingsUI, audioUI, controlsUI;
    private AudioManager myAudioManager;

    private void Start()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void SettingsUIEnable()
    {
        //play click sound
        myAudioManager.Play(Sounds.s_buttonClickUI, true);

        settingsUI.SetActive(true);
        audioUI.SetActive(false);
        controlsUI.SetActive(false);
    }

    public void AudioUIEnable()
    {
        //play click sound
        myAudioManager.Play(Sounds.s_buttonClickUI, true);

        settingsUI.SetActive(false);
        audioUI.SetActive(true);
        controlsUI.SetActive(false);
    }

    public void ControlsUIEnable()
    {
        //play click sound
        myAudioManager.Play(Sounds.s_buttonClickUI, true);

        settingsUI.SetActive(false);
        audioUI.SetActive(false);
        controlsUI.SetActive(true);
    }
}
