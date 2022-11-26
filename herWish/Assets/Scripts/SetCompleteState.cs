using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCompleteState : MonoBehaviour
{
    private DataManager myDataManager;
    bool hasChangedCompleteState = false; 

    private void Start()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
    }

    private void Update()
    {
        if (myDataManager == null)
        {
            myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        }

        if (myDataManager != null && !hasChangedCompleteState) 
        { 
            hasChangedCompleteState= true;
            myDataManager.SetGameComplete(false); 
        }
    }
}
