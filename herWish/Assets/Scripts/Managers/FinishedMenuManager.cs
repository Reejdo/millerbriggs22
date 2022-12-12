using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class FinishedMenuManager : MonoBehaviour
{
    public SettingValues myFinishedSettings;
    public TMP_Text completeText; 
    public TMP_Text[] timerTexts;

    public GameObject[] titleScreens;

    private SaveScriptableData myScriptData;
    private bool hasLoaded; 

    // Start is called before the first frame update
    void Start()
    {


    }

    void Update()
    {
        if (myScriptData == null)
        {
            myScriptData = GameObject.FindObjectOfType<SaveScriptableData>().GetComponent<SaveScriptableData>();
        }
        else if (myScriptData != null)
        {
            if (myScriptData.hasLoadedOnce && !hasLoaded)
            {
                hasLoaded = true; 

                if (myFinishedSettings.GetFinishedGameState())
                {
                    titleScreens[1].SetActive(true);
                    titleScreens[0].SetActive(false);
                }

                SetTimeValues();
                SetCompletedTimeValue();
            }
        }
    }


    void SetTimeValues()
    {
        for (int i = 0; i < myFinishedSettings.GetAllTimerLength(); i++) 
        {
            float timerVal = myFinishedSettings.GetTimerValueAtPosition(i);

            if (timerVal > 0.1f)
            {
                int seconds = (int)(timerVal % 60);
                int minutes = (int)(timerVal / 60) % 60;
                int hours = (int)(timerVal / 3600) % 24;
                int milliseconds = (int)(timerVal * 100) % 100;

                string mainTimerString = string.Format("{0:000}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, milliseconds);

                timerTexts[i].text = mainTimerString;
            }
            else if (timerVal <= 0.1f)
            {
                //Debug.Log("i = " + i); 
                timerTexts[i].text = "---"; 
            }

        }
    }

    void SetCompletedTimeValue()
    {
        completeText.text = "" + myFinishedSettings.GetTimesCompleted(); 
    }
}
