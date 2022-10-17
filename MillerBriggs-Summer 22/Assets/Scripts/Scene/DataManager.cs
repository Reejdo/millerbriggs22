using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string lastLevelLoaded;
    public int lastCheckpoint;
    public List<bool> memoryUnlockStates;  

    [SerializeField]
    private LoadScene myLoadScene;
    [SerializeField]
    //LoadScene MUST be put in the same order as the enum are listed!!
    private UnityEvent[] LoadScene;

    void Awake()
    {
        myLoadScene = GetComponent<LoadScene>(); 

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; //Makes sure no other code is run before destroying gameObject
        }


        DontDestroyOnLoad(gameObject);
    }

    public void ContinueGame()
    {
        string[] sceneNames = System.Enum.GetNames(typeof(SceneLoader.Scene));
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (lastLevelLoaded == sceneNames[i])
            {
                Debug.Log(i); 
                Debug.Log(lastLevelLoaded + " matches " + sceneNames[i]); 
                LoadScene[i - 1].Invoke();
                break; 
            }
        }
    }

    public void SaveGame(string levelName, int checkPoint)
    {
        lastLevelLoaded = levelName;
        lastCheckpoint = checkPoint; 
    }

    /*
    public void LoadToCutscene(int cutSceneNumber)
    {
        myCutSceneNumber = cutSceneNumber;

    }
    */ 

    public void StartNewGame()
    {
        Debug.Log("Start New Game"); 
        lastLevelLoaded = "LevelOne";
        lastCheckpoint = 0;

        //always have first memory unlocked
        for (int i = 0; i < memoryUnlockStates.Count; i++)
        {
            if (i == 0)
            {
                memoryUnlockStates[i] = true; 
            }
            else
            {
                memoryUnlockStates[i] = false;
            }

        }
    }

    public bool GetMemoryState(int memoryNum)
    {
        return memoryUnlockStates[memoryNum]; 
    }
}
