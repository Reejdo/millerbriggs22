using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SaveData : MonoBehaviour
{
    [SerializeField]
    private string currentScene;
    [SerializeField]
    private int thisCheckPointNumber;
    //[SerializeField]
    //private GameObject buttonToDisplay;

    private DataManager myDataManager;
    private DataFileWrite myDataFile;
    public GameObject saveIcon;
    public float saveIconTime = 1f; 

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene().name;
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        myDataFile = GameObject.FindObjectOfType<DataFileWrite>().GetComponent<DataFileWrite>(); 
        //buttonToDisplay.SetActive(false); 
    }

    // Start is called before the first frame update
    void Start()
    {
        if (saveIcon == null)
        {
            saveIcon = GameObject.FindGameObjectWithTag("saveIcon");
            Debug.Log("trying to find save icon!"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myDataManager.SaveGame(currentScene, thisCheckPointNumber);
            myDataFile.UpdateFile(); 
            //buttonToDisplay.SetActive(true); 
            if (!saveIcon.activeSelf)
            {
                StartCoroutine(SaveIcon()); 
            }
        }
    }
    
    IEnumerator SaveIcon()
    {
        saveIcon.SetActive(true);
        yield return new WaitForSeconds(saveIconTime);
        saveIcon.SetActive(false); 
    }

}
