using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public KeyCode pauseKey;

    public GameObject pauseMenuUI;
    
    //we'll use this to disable anything that isn't the default UI right away
    public GameObject[] extraMenuNotDefault;

    public GameObject defaultUI; 
    public GameObject[] disableUIWhenPaused;
    public LogInventory myLogInventory;
    public GameObject completeLogUI;
    public Timer myTimer;
    public UnityEvent LoadMenuScene;

    private AudioManager myAudioManager;
    private bool pressedEscapeToPause; 

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (gameIsPaused)
            {
                //don't want click sound when pressing escape 
                pressedEscapeToPause = true;
                Resume();
            }
            else
            {
                //works out inside of Resume function
                pressedEscapeToPause = false;
                Pause();
            }
        }

    }

    public void Resume()
    { 
        //would be true if player pressed escape to resume, not button
        if (!pressedEscapeToPause)
        {
            //play click sound
            myAudioManager.Play(Sounds.s_buttonClickUI, true);
        }

        pauseMenuUI.SetActive(false);
        ExternalUIState(true); 

        if (myLogInventory.GetLogsCollected() >= myLogInventory.GetMaxLogs())
        {
            completeLogUI.SetActive(true); 
        }

        //Resume time
        Time.timeScale = 1f;
        gameIsPaused = false;

        myTimer.SetTimerState(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        ExternalUIState(false);
        completeLogUI.SetActive(false);
        DisableExtraUIOnPause(); 

        myTimer.SetTimerState(false); 

        //Freeze time
        Time.timeScale = 0f;
        gameIsPaused = true; 
    }

    public void LoadMenu()
    {
        myAudioManager.Play(Sounds.s_buttonClickUI, true);

        Time.timeScale = 1f;
        LoadMenuScene.Invoke(); 
    }

    public void QuitGame()
    {
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        Application.Quit(); 
    }

    void ExternalUIState(bool state)
    {
        foreach (GameObject obj in disableUIWhenPaused)
        {
            obj.SetActive(state); 
        }
    }

    void DisableExtraUIOnPause()
    {
        //make it so when the player pauses, it is always this setup
        defaultUI.SetActive(true); 

        foreach (GameObject obj in extraMenuNotDefault)
        {
            obj.SetActive(false);
        }
    }
}
