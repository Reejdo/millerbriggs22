using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFilePlayerPos : MonoBehaviour
{
    public DataFileWrite myDataFile;
    public DataManager myDataManager; 
    public PlayerControl myPlayerControl;

    public SceneLoader.Scene levelName;

    [SerializeField]
    private bool saveForGround = false;


    private void Start()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        myDataFile = GameObject.FindObjectOfType<DataFileWrite>().GetComponent<DataFileWrite>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForManagers(); 

        if (!myDataManager.GetCompleteState())
        {
            if (myPlayerControl.OneGroundedTrue() && !saveForGround)
            {
                //Debug.Log("Saving on ground hit"); 

                saveForGround = true;
                myDataManager.SaveGame(levelName.ToString(), myPlayerControl.gameObject.transform.position);
                myDataFile.UpdateFile();
            }

            if (!myPlayerControl.OneGroundedTrue() && saveForGround)
            {
                saveForGround = false;
            }
        }
    }

    public void UpdatePlayerInfo()
    {
        myDataManager.SaveGame(levelName.ToString(), myPlayerControl.gameObject.transform.position);
        myDataFile.UpdateFile();
    }

    void CheckForManagers()
    {
        if (myDataManager == null)
        {
            Debug.Log("UpdatePlayerFile Can't find Data Manager!");
            myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        }

        if (myDataFile == null)
        {
            Debug.Log("UpdatePlayerFile Can't find Data Manager!");
            myDataFile = GameObject.FindObjectOfType<DataFileWrite>().GetComponent<DataFileWrite>();
        }
    }
}
