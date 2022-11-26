using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

public class TriggerLoadScene : MonoBehaviour
{
    public UnityEvent LoadScene; 

    private LoadScene myLoadScene;
    //private PlayerControl myPlayerControl; 

    [SerializeField]
    private string playerTag = "Player";
    private bool loadOnce = false;
    [SerializeField]
    private bool isTrigger = false;
    private DataManager myDataManager;
    public GameObject roomFader;
    private float roomFade;
    private Image img;
    private float fadeSpeed = 0.0f;

    private void Awake()
    {
        myLoadScene = GetComponent<LoadScene>();
        myDataManager = GameObject.FindObjectOfType<DataManager>().GetComponent<DataManager>();
        //myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
        img = roomFader.GetComponent<Image>();

        //start room fader as transparent
        img.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        if (roomFader.activeSelf)
        {
            if (img.color.a != 1f)
            {
                Debug.Log("Fade in!");
                roomFade = Mathf.SmoothDamp(img.color.a, 1f, ref fadeSpeed, 1f);
                img.color = new Color(1f, 1f, 1f, roomFade);
            }
        }
    }


    void CallLoadScene()
    {
        LoadScene.Invoke(); 
    }

    public void LoadNextScene()
    {
        StartCoroutine(SceneLoading());
        //Debug.Log("Load next scene"); 
    }

    IEnumerator SceneLoading()
    {
        //myPlayerControl.SetMoveState(false); 
        if (roomFader != null)
        {
            roomFader.SetActive(true);
        }
        yield return new WaitForSeconds(1f); //room fader fades in 1 second
        CallLoadScene();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("trigger"); 
        if (collision.gameObject.CompareTag(playerTag) && !loadOnce)
        {
            //Debug.Log("player collide"); 
            if (isTrigger)
            {
                loadOnce = true;
                LoadNextScene(); 
            }

        }            
    }

}
