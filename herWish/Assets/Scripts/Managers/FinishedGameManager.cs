using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedGameManager : MonoBehaviour
{
    private DataManager myDataManager;
    private DataFileWrite myDataWrite; 
    public SettingValues mySettingValues;

    private bool updatedValues; 

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
        mySettingValues.SetFinishedGameState(true);

        //add another win to the completed values
        mySettingValues.AddCompletion(); 

        //add time value
        mySettingValues.AddNewTime(mySettingValues.GetTimerValue());

        //save changes
        myDataManager.StartNewGame(); 

        //reset level data file to default
        myDataWrite.ResetFileToDefault();

    }


}
