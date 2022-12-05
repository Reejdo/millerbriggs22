using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataFileWrite : MonoBehaviour
{
    public string fileName = "/levelSaveData.txt";
    public List<string> levelName, playerPosX, playerPosY;   
    public string path;
    private string[] allLines;
    private DataManager myDataManager;

    private bool hasLoaded;

    private void Awake()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>(); 
        if (myDataManager != null)
        {
            path = Application.persistentDataPath + "/" + fileName;
            WriteToFile();
        }
        else
        {
            Debug.Log("Data Manager cannot be found!"); 
        }
    }

    public void Start()
    {

    }

    private void Update()
    {
        if (hasLoaded == false)
        {
            LoadGameOnOpenCheck(); 
        }
    }


    public void WriteToFile()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No save file!"); 

            myDataManager.SetPlayerPosition(myDataManager.startX, myDataManager.startY);

            Vector2 playerPos = myDataManager.GetPlayerSavedPosition();

            File.WriteAllText(path, "Save Data File:" + "\n");
            File.AppendAllText(path, myDataManager.defaultScene.ToString() + "," + playerPos.x + "," + playerPos.y + "\n");


            ReadFromFile();

        }
        else
        {
            ReadFromFile();
            File.WriteAllLines(path, allLines);
        }

    }


    public void UpdateFile()
    {
        if (allLines.Length != 0)
        {
            Vector2 playerPos = myDataManager.GetPlayerSavedPosition();

            allLines[1] = myDataManager.lastLevelLoaded + "," + playerPos.x + "," + playerPos.y; 
            File.WriteAllLines(path, allLines);
        }
        else
        {
            Debug.Log("All Lines is empty!"); 
        }
    }


    public void ReadFromFile()
    {
        allLines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, path));
        //Start at 1 to skip intro line
        for (int i = 1; i < allLines.Length; i++)
        {
            string[] thisString = allLines[i].Split(char.Parse(","));
            //Debug.Log(thisString[0] + " " + thisString[1]); 
            levelName.Add(thisString[0]);
            playerPosX.Add(thisString[1]);
            playerPosY.Add(thisString[2]);
        }

        //Update the Data Manager
        myDataManager.lastLevelLoaded = levelName[0];
        float xPos = float.Parse(playerPosX[0]);
        float yPos = float.Parse(playerPosY[0]);

        myDataManager.SetPlayerPosition(xPos, yPos);

        myDataManager.SetPosLoadState(true); 
    }

    public void ResetFileToDefault()
    {
        allLines[1] = "Scene_1" + "," + myDataManager.startX + "," + myDataManager.startY;
        File.WriteAllLines(path, allLines);
    }


    void LoadGameOnOpenCheck()
    {
        if (myDataManager.HasCheckedOpen())
        {
            hasLoaded = true; 
            myDataManager.mySaveData.LoadGame();
        }
        else
        {
            Debug.Log("Has not checked first open!");
        }
    }
}
