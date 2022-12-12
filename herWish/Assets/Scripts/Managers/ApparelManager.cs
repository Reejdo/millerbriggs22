using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparelManager : MonoBehaviour
{
    [SerializeField] private SettingValues myCompleteSettings;
    [SerializeField] private GameObject[] hatObject;

    bool enabledHats; 

    // Start is called before the first frame update
    void Start()
    {
        DisableAllHats(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabledHats)
        {
            enabledHats = true;
            CheckHats(); 
        }
    }


    void DisableAllHats()
    {
        foreach (GameObject obj in hatObject)
        {
            obj.SetActive(false); 
        }
    }

    void CheckHats()
    {
        if (myCompleteSettings.GetFinishedGameState())
        {
            EnableHats(); 
        }
        else
        {
            DisableAllHats(); 
        }
    }


    void EnableHats()
    {
        if (myCompleteSettings.GetTimesCompleted() > 0)
        {
            hatObject[0].SetActive(true); 
        }
    }
}
