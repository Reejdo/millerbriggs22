using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedGameManager : MonoBehaviour
{
    private DataManager myDataManager;
    private DataFileWrite myDataWrite; 
    public SettingValues mySettingValues;
    public CompleteGameSettings myFinishedSettings;

    private bool updatedValues; 

    // Start is called before the first frame update
    void Start()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        myDataWrite = GameObject.FindObjectOfType<DataFileWrite>().GetComponent<DataFileWrite>();


        if (myDataManager != null && myDataWrite != null)
        {
            updatedValues = true;
            UpdateValues();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (myDataManager == null || myDataWrite == null)
        {
            myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
            myDataWrite = GameObject.FindObjectOfType<DataFileWrite>().GetComponent<DataFileWrite>();
        }

        if (!updatedValues && myDataManager != null && myDataWrite != null)
        {
            updatedValues = true;
            UpdateValues();
        }
    }


    void UpdateValues() 
    {
        //set finished game state to true
        myFinishedSettings.SetFinishedGameState(true);

        //add another win to the completed values
        myFinishedSettings.AddCompletion(); 

        //add time value
        myFinishedSettings.AddNewTime(mySettingValues.GetTimerValue());

        //save changes
        myDataManager.StartNewGame(); 

        //reset level data file to default
        myDataWrite.ResetFileToDefault();

    }


}
