using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultButtons, continueButtons;

    public string defaultLevelToLoad = "HouseOne"; 
    private DataManager myDataManager;

    // Start is called before the first frame update
    private void Awake()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        continueButtons.SetActive(false);
        defaultButtons.SetActive(false); 

    }

    void Start()
    {
        if (myDataManager.lastLevelLoaded != defaultLevelToLoad)
        {
            continueButtons.SetActive(true);
            defaultButtons.SetActive(false); 
        }
        else
        {
            continueButtons.SetActive(false);
            defaultButtons.SetActive(true); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
