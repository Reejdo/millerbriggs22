using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

public class CompletionManager : MonoBehaviour
{
    public LogInventory myLogInventory;
    public GameObject completeUI;
    public UnityEvent LoadScene;

    private DataManager myDataManager;
    private DataFileWrite myDataFileWrite; 
    private PlayerControl myPlayerControl;
    private AudioManager myAudioManager; 
    public GameObject roomFader;
    public ReactDialogues myReactDialogue;
    public Sounds completionSound;
    private float roomFade;
    private Image img;
    private float fadeSpeed = 0.0f;
    private float roomFadeTime = 1f;
    private bool roomFadeIn, uiEnabled;

    [SerializeField]
    private GameObject playerObject; 

    // Start is called before the first frame update
    void Start()
    {
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        myDataFileWrite = GameObject.FindObjectOfType<DataFileWrite>().GetComponent<DataFileWrite>(); 
        myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerObject = GameObject.Find("player"); 
        img = roomFader.GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (myAudioManager == null)
        {
            myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }


        if (roomFadeIn)
        {
            if (img.color.a != 1f)
            {
                //Debug.Log("Fade in!");
                roomFade = Mathf.SmoothDamp(img.color.a, 1f, ref fadeSpeed, 1f);
                img.color = new Color(1f, 1f, 1f, roomFade);
            }
        }

        if (myLogInventory.GetLogsCollected() >= myLogInventory.GetMaxLogs() && !uiEnabled)
        {
            uiEnabled = true; 
            completeUI.SetActive(true);
            myReactDialogue.TriggerDialogue(0);
            //play some audio queue
            myAudioManager.Play(completionSound, false);
        }

        else if (myLogInventory.GetLogsCollected() < myLogInventory.GetMaxLogs())
        {
            uiEnabled = false;
            completeUI.SetActive(false);
        }
    }

    void CallLoadScene()
    {
        myDataManager.SetNextLevelValues();
        Debug.Log("Before load position: " + myDataManager.GetPlayerSavedPosition());
        Debug.Log("Last level loaded: " + myDataManager.lastLevelLoaded); 
        myDataFileWrite.UpdateFile(); 
        LoadScene.Invoke();
    }

    public void CompleteScene()
    {
        myAudioManager.Play(Sounds.s_buttonClickUI, true);
        StartCoroutine(SceneLoading());
        //Debug.Log("Load next scene"); 
    }

    IEnumerator SceneLoading()
    {
        myPlayerControl.SetMoveState(false); 
        myDataManager.SetGameComplete(true);

        if (roomFader != null)
        {
            roomFader.SetActive(true);
        }
        roomFadeIn = true; 

        yield return new WaitForSeconds(roomFadeTime);
        playerObject.SetActive(false);
        CallLoadScene();
    }




}
