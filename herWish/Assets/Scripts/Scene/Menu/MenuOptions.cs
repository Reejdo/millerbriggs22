using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
    public SaveScriptableData mySaveData;


    private void Awake()
    {
        mySaveData = GameObject.FindObjectOfType<SaveScriptableData>().GetComponent<SaveScriptableData>(); 
    }

    private void Update()
    {
        if (mySaveData == null)
        {
            mySaveData = GameObject.FindObjectOfType<SaveScriptableData>().GetComponent<SaveScriptableData>();
        }
    }

    public void QuitGame()
    {
        mySaveData.SaveGame(); 

        Application.Quit();
    }
}
