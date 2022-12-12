using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public SceneLoader.Scene defaultScene; 
    public string lastLevelLoaded;
    [SerializeField]
    private float playerPosX, playerPosY;
    public float startX = -137.11f, startY = -0.212f;
    private bool playerPosLoaded; 

    [SerializeField]
    private LoadScene myLoadScene;
    [SerializeField]
    //LoadScene MUST be put in the same order as the enum are listed!!
    private UnityEvent[] LoadScene;

    [Header("First Time Save Check")]
    public FirstTimeCheck myFirstCheck; 


    [Header("ScriptableObjects")]
    public AllDialogueObjects allDialogues;
    public LogInventory logInventory;
    public SettingValues settingValues; 
    public SaveScriptableData mySaveData;

    [SerializeField]
    private int targetFrames = 60; 

    //Number for cutScene_1 in DataManager's load scene list
    private int cutScene_1_Number = 4;

    private bool hasCheckedFirstOpen = false; 
    private bool gameComplete; 

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

        //Set targeted frame rate
        Application.targetFrameRate = targetFrames; 


        //if it is first time opening, set everything to default
        if (myFirstCheck.FirstTime())
        {
            Debug.Log("Start New Game");
            lastLevelLoaded = "Scene_1";
            SetPlayerPosition(startX, startY);
            SetGameComplete(false);

            settingValues.ResetToDefault();
            logInventory.ResetToDefault();
            allDialogues.ResetAllDialogues();
            settingValues.ResetTimer();
            mySaveData.SaveGame();
            hasCheckedFirstOpen = true; 
        }
        else
        {
            //Debug.Log("Has opened"); 
            hasCheckedFirstOpen = true; 
        }
    }

    public void ContinueGame()
    {
        if (CheckContinueGame())
        {
            mySaveData.LoadGame();

            string[] sceneNames = System.Enum.GetNames(typeof(SceneLoader.Scene));
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (lastLevelLoaded == sceneNames[i])
                {
                    //Debug.Log(i); 
                    //Debug.Log(lastLevelLoaded + " matches " + sceneNames[i]); 
                    LoadScene[i - 1].Invoke();
                    break;
                }
            }
        }

        else
        {
            StartNewGame();
            LoadScene[cutScene_1_Number].Invoke(); 
        }

    }

    public void SaveGame(string levelName, Vector2 playerPos)
    {
        lastLevelLoaded = levelName;

        SetPlayerPosition(playerPos.x, playerPos.y); 

        //Debug.Log("Data Manager - Save Game Data");
        mySaveData.SaveGame(); 
    }


    public void StartNewGame()
    {
        Debug.Log("Start New Game"); 
        lastLevelLoaded = "Scene_1";
        SetPlayerPosition(startX, startY);
        SetGameComplete(false); 

        logInventory.ResetToDefault();
        allDialogues.ResetAllDialogues(); 
        settingValues.ResetTimer();
        mySaveData.SaveGame();
        
    }

    public void FinishGame()
    {
        Debug.Log("Finish Game");
        SetPlayerPosition(startX, startY);
        SetGameComplete(false);

        logInventory.ResetToDefault();
        allDialogues.ResetAllDialogues();
        settingValues.ResetTimer();
        mySaveData.SaveGame();
    }

    public void SetNextLevelValues()
    {
        //Debug.Log("Set Next Level Values");
        lastLevelLoaded = "Scene_2";
        SetPlayerPosition(startX, startY);
        mySaveData.SaveGame();
    }

    public void SetPlayerPosition(float posX, float posY)
    {
        //Debug.Log("Set Player Position");
        playerPosX = posX;
        playerPosY = posY;
    }

    public void SetPosLoadState(bool state)
    {
        //once done loading positions from file, set this to true for teleport on load
        playerPosLoaded = state;
    }

    public bool GetPosLoadState()
    {
        return playerPosLoaded; 
    }

    public Vector2 GetPlayerSavedPosition()
    {
        return new Vector2(playerPosX, playerPosY); 
    }

    public void SetGameComplete(bool state)
    {
        gameComplete = state; 
    }

    public bool GetCompleteState()
    {
        return gameComplete; 
    }

    bool CheckContinueGame()
    {
        Vector2 startLocation = new Vector2(startX, startY); 
        Vector2 savedLocation = GetPlayerSavedPosition();

        if ((startLocation.x == savedLocation.x) && (startLocation.y == savedLocation.y))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool HasCheckedOpen()
    {
        return hasCheckedFirstOpen; 
    }
}
