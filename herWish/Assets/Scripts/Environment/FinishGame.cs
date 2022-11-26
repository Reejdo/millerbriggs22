using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour
{
    public UnityEvent LoadScene;
    public GameObject roomFader;
    private float roomFade;
    private Image img;
    private float fadeSpeed = 0.0f;
    private float roomFadeTime = 2f;
    private bool roomFadeIn, uiEnabled;
    private bool startLoad; 

    private DataManager myDataManager;
    private DataFileWrite myDataFileWrite;
    private PlayerControl myPlayerControl;
    private Timer myTimer; 

    // Start is called before the first frame update
    void Start()
    {
        img = roomFader.GetComponent<Image>();
        myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
        myTimer = GameObject.FindObjectOfType<Timer>().GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayerControl == null)
        {
            Debug.Log("Finding player control!"); 
            myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
        }

        if (roomFadeIn)
        {
            if (img.color.a != 1f)
            {
                //Debug.Log("Fade in!");
                roomFade = Mathf.SmoothDamp(img.color.a, 1f, ref fadeSpeed, 1f);
                img.color = new Color(0f, 0f, 0f, roomFade);
            }
        }
    }

    IEnumerator SceneLoading()
    {
        myTimer.SetTimePermanentStop(true); 
        myPlayerControl.SetMoveState(false);

        if (roomFader != null)
        {
            img.color = new Color(0f, 0f, 0f, 0f);
            roomFader.SetActive(true);
        }
        roomFadeIn = true;

        yield return new WaitForSeconds(roomFadeTime);
        
        
        CallLoadScene();
    }


    void CallLoadScene()
    {
        LoadScene.Invoke();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !startLoad)
        {
            startLoad = true; 
            StartCoroutine(SceneLoading());
        }
    }


}
