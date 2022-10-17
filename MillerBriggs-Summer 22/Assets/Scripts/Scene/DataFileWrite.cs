using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataFileWrite : MonoBehaviour
{
    public string fileName = "/levelSaveData.txt";
    public List<string> levelName, checkpointNumber, memoryStates; 
    public string path;
    private string[] allLines;
    private DataManager myDataManager; 

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


    public void WriteToFile()
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Save Data File:" + "\n");
            File.AppendAllText(path, myDataManager.lastLevelLoaded + "," + myDataManager.lastCheckpoint + "\n");

            for (int i = 0; i < myDataManager.memoryUnlockStates.Count; i++)
            {
                File.AppendAllText(path, myDataManager.memoryUnlockStates[i].ToString() + "\n");
            }

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
            allLines[1] = myDataManager.lastLevelLoaded + "," + myDataManager.lastCheckpoint;

            for (int i = 2; i < allLines.Length; i++)
            {
                Debug.Log("Update file for diary states");
                allLines[i] = myDataManager.memoryUnlockStates[i - 2].ToString();
            }

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
            if (i == 1)
            {
                string[] thisString = allLines[i].Split(char.Parse(","));
                //Debug.Log(thisString[0] + " " + thisString[1]); 
                levelName.Add(thisString[0]);
                checkpointNumber.Add(thisString[1]);
            }
            if (i >= 2)
            {
                //Debug.Log("Reading to memory"); 
                memoryStates.Add(allLines[i]);

                //Updates Data Manager
                if (memoryStates[i - 2] == "True")
                {
                    //Debug.Log("True"); 
                    myDataManager.memoryUnlockStates[i - 2] = true;
                }
                else
                {
                    //Debug.Log("False");
                    myDataManager.memoryUnlockStates[i - 2] = false;
                }

            }
        }

        //Update the Data Manager
        myDataManager.lastLevelLoaded = levelName[0];
        myDataManager.lastCheckpoint = int.Parse(checkpointNumber[0]);
    }

    public void ResetFileToDefault()
    {
        allLines[1] = "LevelOne" + "," + 0;
        for (int i = 2; i < allLines.Length; i++)
        {
            allLines[i] = "False"; 
        }
        File.WriteAllLines(path, allLines);
    }

}
