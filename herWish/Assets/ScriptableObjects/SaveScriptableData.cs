using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveScriptableData : MonoBehaviour
{
    //Connect this script to datamanager, no need to make singleton here

    //Scriptable Objects below:
    public LogInventory logInventory;
    public MovePlatforms movePlatform, tempMovePlatform;
    public AllDialogueObjects allDialogues, tempDialogues;
    public SettingValues settingValues, tempSettings;

    private bool currentlySaving;
    private bool currentlyLoading;

    public bool hasLoadedOnce; 

    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/data_Save"); 
    }

    public void SaveGame()
    {
        if (!currentlySaving && !currentlyLoading)
        {
            Debug.Log("Saving!"); 

            //Only one save at a time
            currentlySaving = true; 

            if (!IsSaveFile())
            {
                Debug.Log("No scriptable saves found -- creating location");
                Directory.CreateDirectory(Application.persistentDataPath + "/data_Save");
            }

            BinaryFormatter bf = new BinaryFormatter();

            //Log Data
            try
            {
                FileStream file_1 = File.Create(Application.persistentDataPath + "/data_Save/lgIv_Data");
                var json_1 = JsonUtility.ToJson(logInventory);
                bf.Serialize(file_1, json_1);
                file_1.Close();
            }
            catch
            {
                Debug.Log("Error saving lgIV"); 
            }

            //Move Platform Data
            try
            {
                FileStream file_2 = File.Create(Application.persistentDataPath + "/data_Save/mvPl_Data");
                var json_2 = JsonUtility.ToJson(movePlatform);
                bf.Serialize(file_2, json_2);
                file_2.Close();
            }
            catch
            {
                Debug.Log("Error saving mvPl"); 
            }

            //All Dialogue Data
            try
            {
                FileStream file_3 = File.Create(Application.persistentDataPath + "/data_Save/aD_Data");
                var json_3 = JsonUtility.ToJson(allDialogues);
                bf.Serialize(file_3, json_3);
                file_3.Close();
            }
            catch
            {
                Debug.Log("Error saving aD"); 
            }

            //Settings Data
            try
            {
                FileStream file_4 = File.Create(Application.persistentDataPath + "/data_Save/settings");
                var json_4 = JsonUtility.ToJson(settingValues);
                bf.Serialize(file_4, json_4);
                file_4.Close();
            }
            catch
            {
                Debug.Log("Error saving settings"); 
            }

            //F-Settings Data
            /*
            try
            {
                FileStream file_5 = File.Create(Application.persistentDataPath + FLOCATION + "/" + FFILENAME);
                var json_5 = JsonUtility.ToJson(finishedValues);
                bf.Serialize(file_5, json_5);
                file_5.Close();
            }
            catch
            {
                Debug.Log("Error saving f_settings"); 
            }
            */

            hasLoadedOnce = true; 

            currentlySaving = false; 
        }
    }

    public void LoadGame()
    {
        if (!currentlyLoading && !currentlySaving)
        {
            currentlyLoading = true;

            Debug.Log("Loading"); 

            //All files should be created, so only need to check if 1 doesn't exist
            if (!Directory.Exists(Application.persistentDataPath + "/data_Save"))
            {
                LoadCallSave(); 
            }

            //Makes sure each file exists
            CheckFilesExist();

            //Make sure each file has the same number of data types
            CompareBeforeLoad();

            //Debug.Log("Load Scriptable Data");

            if (!currentlySaving)
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (File.Exists(Application.persistentDataPath + "/data_Save/lgIv_Data"))
                {
                    FileStream file_1 = File.Open(Application.persistentDataPath + "/data_Save/lgIv_Data", FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_1), logInventory);
                    file_1.Close();
                }
                if (File.Exists(Application.persistentDataPath + "/data_Save/mvPl_Data"))
                {
                    FileStream file_2 = File.Open(Application.persistentDataPath + "/data_Save/mvPl_Data", FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_2), movePlatform);
                    file_2.Close();
                }
                if (File.Exists(Application.persistentDataPath + "/data_Save/aD_Data"))
                {
                    FileStream file_3 = File.Open(Application.persistentDataPath + "/data_Save/aD_Data", FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_3), allDialogues);
                    file_3.Close();
                }
                if (File.Exists(Application.persistentDataPath + "/data_Save/settings"))
                {
                    FileStream file_4 = File.Open(Application.persistentDataPath + "/data_Save/settings", FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_4), settingValues);
                    file_4.Close();
                }
                /*
                if (File.Exists(Application.persistentDataPath + FLOCATION + "/" + FFILENAME))
                {
                    FileStream file_5 = File.Open(Application.persistentDataPath + FLOCATION + "/" + FFILENAME, FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_5), finishedValues);
                    file_5.Close();
                }
                */ 
            }

            hasLoadedOnce = true; 

            currentlyLoading = false;
        }
    }

    void CheckFilesExist()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (!File.Exists(Application.persistentDataPath + "/data_Save/lgIv_Data"))
        {
            FileStream file_1 = File.Create(Application.persistentDataPath + "/data_Save/lgIv_Data");
            var json_1 = JsonUtility.ToJson(logInventory);
            bf.Serialize(file_1, json_1);
            file_1.Close();
        }
        if (!File.Exists(Application.persistentDataPath + "/data_Save/mvPl_Data"))
        {
            FileStream file_2 = File.Create(Application.persistentDataPath + "/data_Save/mvPl_Data");
            var json_2 = JsonUtility.ToJson(movePlatform);
            bf.Serialize(file_2, json_2);
            file_2.Close(); 
        }
        if (!File.Exists(Application.persistentDataPath + "/data_Save/aD_Data"))
        {
            FileStream file_3 = File.Create(Application.persistentDataPath + "/data_Save/aD_Data");
            var json_3 = JsonUtility.ToJson(allDialogues);
            bf.Serialize(file_3, json_3);
            file_3.Close();
        }
        if (!File.Exists(Application.persistentDataPath + "/data_Save/settings"))
        {
            FileStream file_4 = File.Create(Application.persistentDataPath + "/data_Save/settings");
            var json_4 = JsonUtility.ToJson(settingValues);
            bf.Serialize(file_4, json_4);
            file_4.Close();
        }
        /*
        if (!File.Exists(Application.persistentDataPath + FLOCATION + "/" + FFILENAME))
        {
            FileStream file_5 = File.Create(Application.persistentDataPath + FLOCATION + "/" + FFILENAME);
            var json_5 = JsonUtility.ToJson(finishedValues);
            bf.Serialize(file_5, json_5);
            file_5.Close();
        }
        */ 
    }

    void CompareBeforeLoad()
    {
        //If this is true by the end, settings aren't updated, so save first
        bool flagForSave = false; 

        BinaryFormatter bf = new BinaryFormatter();

        if (!flagForSave)
        {
            if (File.Exists(Application.persistentDataPath + "/data_Save/lgIv_Data"))
            {
                FileStream file_1 = File.Open(Application.persistentDataPath + "/data_Save/lgIv_Data", FileMode.Open);

                try
                {
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_1), logInventory);

                }
                catch
                {
                    Debug.Log("Files are different - saving instead");
                    flagForSave = true;
                }

                file_1.Close();
            }
            else
            {
                flagForSave = true;
            }

            if (File.Exists(Application.persistentDataPath + "/data_Save/mvPl_Data"))
            {
                FileStream file_2 = File.Open(Application.persistentDataPath + "/data_Save/mvPl_Data", FileMode.Open);

                try
                {
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_2), tempMovePlatform);
                }
                catch
                {
                    Debug.Log("Files are different - saving instead");
                    flagForSave = true;
                }

                file_2.Close();

                if (!flagForSave)
                {
                    if (tempMovePlatform.platformPositions.Length != movePlatform.platformPositions.Length)
                    {
                        Debug.Log("Platform number is different!");

                        //make sure to update the ones that do exist
                        for (int i = 0; i < tempMovePlatform.platformPositions.Length; i++)
                        {
                            movePlatform.platformPositions[i] = tempMovePlatform.platformPositions[i];
                        }

                        flagForSave = true;
                    }

                    if (tempMovePlatform.playerParented.Length != movePlatform.playerParented.Length)
                    {
                        //make sure to update the ones that do exist
                        for (int i = 0; i < tempMovePlatform.playerParented.Length; i++)
                        {
                            movePlatform.playerParented[i] = tempMovePlatform.playerParented[i];
                        }

                        flagForSave = true;
                    }
                }
            }
            else
            {
                flagForSave = true;
            }

            if (File.Exists(Application.persistentDataPath + "/data_Save/aD_Data"))
            {
                FileStream file_3 = File.Open(Application.persistentDataPath + "/data_Save/aD_Data", FileMode.Open);

                try
                {
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_3), tempDialogues);
                }
                catch
                {
                    Debug.Log("Files are different - saving instead");
                    flagForSave = true;
                }

                file_3.Close();

                if (!flagForSave)
                {
                    if (tempDialogues.hasActivated.Length != allDialogues.hasActivated.Length)
                    {
                        Debug.Log("Dialogue length is different!");

                        flagForSave = true;
                    }
                }

            }
            else
            {
                flagForSave = true;
            }

            if (File.Exists(Application.persistentDataPath + "/data_Save/settings"))
            {
                FileStream file_4 = File.Open(Application.persistentDataPath + "/data_Save/settings", FileMode.Open);

                try
                {
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_4), tempSettings);
                }
                catch
                {
                    Debug.Log("Files are different - saving instead");
                    flagForSave = true;
                }

                file_4.Close();


                if (tempSettings.soundVolumes.Length != settingValues.soundVolumes.Length)
                {
                    Debug.Log("Number of sound volumes is different!");

                    flagForSave = true;

                }

                if (tempSettings.GetTimerValue() == 0)
                {
                    Debug.Log("No timer!");
                    flagForSave = true;
                }

            }
            else
            {
                flagForSave = true;
            }

            /*
            if (File.Exists(Application.persistentDataPath + FLOCATION + "/" + FFILENAME))
            {
                FileStream file_5 = File.Open(Application.persistentDataPath + FLOCATION + "/" + FFILENAME, FileMode.Open);

                try
                {
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_5), tempFinValues);
                }
                catch
                {
                    Debug.Log("Files are different - saving instead");
                    flagForSave = true;
                }
            }
            else
            {
                flagForSave = true; 
            }
            */ 
        }


        //Save the game first before loading so that 
        if (flagForSave) 
        {
            Debug.Log("Flagged to Save");
            LoadCallSave(); 
        }

    }

    void LoadCallSave()
    {
        if (!currentlySaving)
        {
            currentlyLoading = false;
            SaveGame();
        }
        else
        {
            Debug.Log("Already saving!"); 
        }
    }


    public bool IsCurrentlySaving()
    {
        return currentlySaving; 
    }
}
