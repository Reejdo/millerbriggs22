using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MemoryCard : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("MEMORY NUMBER IN LIST")]
    public int memoryNumber;
    public bool myMemoryState; 
    private DataManager myDataManger;
    public GameObject loadLevelObject; 

    [Header("Sprite Data")]
    private SpriteRenderer mySr; 
    //1st sprite is null sprite, second is actual sprite
    public Sprite enabledSprite, disabledSprite;
    public Light2D spriteLight;
    public float lightIntensity; 


    void Start()
    {
        myDataManger = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>(); 

        if (myDataManger != null)
        {
            myMemoryState = myDataManger.GetMemoryState(memoryNumber); 
        }

        mySr = GetComponent<SpriteRenderer>(); 


        spriteLight.intensity = 0;
        mySr.sprite = disabledSprite;
        loadLevelObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (myDataManger == null)
        {
            myDataManger = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();

            if (myDataManger != null)
            {
                myMemoryState = myDataManger.GetMemoryState(memoryNumber);
            }
        }

        if (myMemoryState)
        {
            spriteLight.intensity = lightIntensity;
            mySr.sprite = enabledSprite;
            loadLevelObject.SetActive(true); 
        }
        else
        {
            Debug.Log("memory not active"); 
            spriteLight.intensity = 0;
            mySr.sprite = disabledSprite;
            loadLevelObject.SetActive(false);
        }
    }
}
