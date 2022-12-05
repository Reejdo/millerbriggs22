using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FirstTimeCheck : MonoBehaviour
{
    public static FirstTimeSave firstSave = new FirstTimeSave();
    public const string FileLocation = "/ftc";  
    public const string FileName = "ftc.sav";

    BinaryFormatter bf = new BinaryFormatter();

    private bool savingFile, hasUpdatedFile; 

    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + FileLocation);
    }

    public bool FirstTime()
    {
        if (!IsSaveFile())
        {
            Debug.Log("No save location found -- creating location");
            Directory.CreateDirectory(Application.persistentDataPath + FileLocation);
        }

        //If the file already exists we've opened the game before, so it's not the first time
        if (DoesFileExist())
        {
            return false; 
        }
        else
        {
            //if first time save is false, return true as it is the first time game is being opened, then set to true
            Debug.Log("First time opening!");
            firstSave.SetFTS(true); 
            CreateFile();
            return true; 
        }
    }

    private void Update()
    {
        if (hasUpdatedFile)
        {
            if (!savingFile)
            {
                CreateFile();
                hasUpdatedFile = false; 
            }
        }
    }


    bool DoesFileExist()
    {
        if (!File.Exists(Application.persistentDataPath + FileLocation + "/" + FileName))
        {
            Debug.Log("FTC File Does not exist!"); 
            return false; 
        }
        else
        {
            //Debug.Log("FTC File exists"); 
            return true;  
        }
    }

    /*
    void CompareFile()
    {
        bool flagForSave = false; 

        if (File.Exists(Application.persistentDataPath + FileLocation + "/" + FileName))
        {
            FileStream file_1 = File.Open(Application.persistentDataPath + FileLocation + "/" + FileName, FileMode.Open);

            try
            {
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file_1), firstSave);
            }
            catch
            {
                Debug.Log("Files are different - creating file instead");
                flagForSave = true;
            }
        }

        //Save the game first before loading so that 
        if (flagForSave)
        {
            CreateFile();
        }
    }
    */

    void CreateFile()
    {
        savingFile = true; 
        Debug.Log("Saving FTS file"); 
        FileStream file_1 = File.Create(Application.persistentDataPath + FileLocation + "/" + FileName);
        var json_1 = JsonUtility.ToJson(firstSave, prettyPrint: true);
        bf.Serialize(file_1, json_1);
        file_1.Close();
        savingFile = false; 
    }
}
