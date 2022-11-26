using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public SettingValues mySettingValues;

    private float timerVal;

    public GameObject timerUI; 
    [SerializeField] private TMP_Text mainText, millisecondText; 

    bool timerRunning = true;
    bool timerPermanentStop = false; 

    void Start()
    {
        timerUI.SetActive(false);

        timerVal = mySettingValues.GetTimerValue(); 
    }

    // Update is called once per frame
    void Update()
    {

        if (timerRunning && !timerPermanentStop)
        {
            UpdateTimerDisplay(); 
        }

        ChangeTimerEnabledState();
    }

    void UpdateTimerDisplay()
    {
        timerVal += Time.deltaTime;

        mySettingValues.SetTimerValue(timerVal);

        int seconds = (int)(timerVal % 60); 
        int minutes = (int)(timerVal / 60) % 60;
        int hours = (int)(timerVal / 3600) % 24;
        int milliseconds = (int)(timerVal * 100) % 100;

        string mainTimerString = string.Format("{0:000}:{1:00}:{2:00}", hours, minutes, seconds);
        string milliTimerString = ":" + milliseconds; 

        mainText.text = mainTimerString;
        millisecondText.text = milliTimerString;
    }


    public void SetTimerState(bool state)
    {
        timerRunning = state;
    }

    void ChangeTimerEnabledState()
    {
        if (mySettingValues.timerOn)
        {
            timerUI.SetActive(true);
        }
        else
        {
            timerUI.SetActive(false);
        }
    }

    public void SetTimePermanentStop(bool state)
    {
        timerPermanentStop = state; 
    }
}
