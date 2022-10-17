using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
public class NewGame : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityEvent myEvent;
    private DataManager myDataManager;
    private DataFileWrite myFileWrite; 

    void Start()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        myFileWrite = GameObject.FindObjectOfType<DataFileWrite>().GetComponent<DataFileWrite>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        myDataManager.StartNewGame(); 
        myFileWrite.ResetFileToDefault(); 
        myEvent.Invoke(); 
    }
}
