using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGame : MonoBehaviour
{

    // Start is called before the first frame update
    private DataManager myDataManager;

    void Start()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (myDataManager == null)
        {
            Debug.Log("Data manager null");
            FindDataManager(); 
        }
    }

    public void LoadContinueGame()
    {
        myDataManager.ContinueGame(); 
    }

    void FindDataManager()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
    }

}
