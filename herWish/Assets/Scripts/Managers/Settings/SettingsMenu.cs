using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SettingsMenu : MonoBehaviour
{
    public SettingValues mySettingValues;
    public GameObject[] visualEffects;
    public Toggle visualToggle, fullScreenToggle, dialogueToggle, timerToggle;

    public DialogueManager myDialogueManager;
    private SaveScriptableData mySaveData;
    private AudioManager myAudioManager;

    private float dontPlaySoundTime = 0.5f, dontPlaySoundTimer;

    [SerializeField] private bool isMainMenu; 

    // Start is called before the first frame update
    void Start()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        
        if (!isMainMenu)
        {
            myDialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        }
 
        mySaveData = GameObject.FindObjectOfType<SaveScriptableData>().GetComponent<SaveScriptableData>();

        visualToggle.isOn = mySettingValues.visualEffects;
        fullScreenToggle.isOn = mySettingValues.fullScreen;
        dialogueToggle.isOn = mySettingValues.dialogueOn;
        timerToggle.isOn = mySettingValues.timerOn;

        //Set values at start
        Screen.fullScreen = fullScreenToggle.isOn;

        if (myDialogueManager != null)
        {
            myDialogueManager.SetDialogueActiveState(dialogueToggle.isOn);
        }

        SetVisualEffects(visualToggle.isOn); 
    }

    private void Update()
    {
        //Makes sure click sound doesn't play right away
        if (dontPlaySoundTimer < dontPlaySoundTime)
        {
            dontPlaySoundTimer += Time.deltaTime; 
        }
    }


    public void ChangeVisualToggle()
    {
        PlayToggleSound();

        bool toggle;
        toggle = visualToggle.isOn;
        
        mySettingValues.visualEffects = toggle;

        SetVisualEffects(toggle);

        SaveOnToggleChange(); 
    }

    public void ChangeFullScreenToggle() 
    {
        PlayToggleSound(); 

        bool toggle;
        toggle = fullScreenToggle.isOn;

        mySettingValues.fullScreen = toggle;

        Screen.fullScreen = toggle;

        SaveOnToggleChange(); 
    }

    public void ChangeDialogueToggle()
    {
        PlayToggleSound();

        bool toggle;
        toggle = dialogueToggle.isOn;

        mySettingValues.dialogueOn = toggle;

        if (myDialogueManager != null)
        {
            myDialogueManager.SetDialogueActiveState(toggle);
        }

        SaveOnToggleChange(); 
    }

    public void ChangeTimerToggle()
    {
        //Debug.Log("Change timer toggle"); 

        PlayToggleSound(); 

        bool toggle;
        toggle = timerToggle.isOn; 

        mySettingValues.timerOn = toggle;

        SaveOnToggleChange(); 

    }

    void PlayToggleSound()
    {
        if (dontPlaySoundTimer >= dontPlaySoundTime)
        {
            myAudioManager.Play(Sounds.s_buttonClickUI, true);
        }
    }


    void SaveOnToggleChange()
    {
        //Use this as a save timer as well so no overlapping saving occurs
        if (dontPlaySoundTimer >= dontPlaySoundTime)
        {
            if (!mySaveData.IsCurrentlySaving())
            {
                mySaveData.SaveGame();
            }
        }
    }


    void SetVisualEffects(bool toggle)
    {
        foreach (GameObject obj in visualEffects)
        {
            obj.SetActive(toggle);
        }
    }
}
