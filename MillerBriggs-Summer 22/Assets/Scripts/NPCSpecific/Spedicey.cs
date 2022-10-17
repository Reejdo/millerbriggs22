using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spedicey : MonoBehaviour
{
    public DialogueTrigger myDialogueTrigger;
    public DialogueManager myDialogueManager;
    public AudioManager myAudioManager;
    public ThisLevelMusic myMusic;
    public PlayerControl pControl; 
    public Animator spediceyAnim, playerAnim;
    
    public GameObject dialogueTriggerObj, danceVisualCue;
    public SpriteRenderer firstVisualImg;
 
    private float fadeTime = 1f;
    private float fadeSpeed = 0.0f;
    private bool playerInRange;
    public bool danceMode;
    private bool isDancing;
    private bool axisInUse; 

    // Start is called before the first frame update
    void Start()
    {
        danceVisualCue.SetActive(false);
        myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (myDialogueTrigger.doneWithOneDialogue && dialogueTriggerObj.activeInHierarchy)
        {
            dialogueTriggerObj.SetActive(false);

            //fade out the first visual, the script to fade it has been disabled
            StartCoroutine(FadeFirstVisual());

            danceMode = true; 
            danceVisualCue.SetActive(true);
        }

        CheckDancing(); 

        if (pControl.GetIsMoving())
        {
            isDancing = false; 
        }

        spediceyAnim.SetBool("isDancing", isDancing);
        playerAnim.SetBool("isDancing", isDancing); 
    }


    IEnumerator FadeFirstVisual()
    {
        for (float fade = 1f; fade > 0; fade -= 0.1f)
        {
            firstVisualImg.color = new Color(1f, 1f, 1f, fade);
            yield return new WaitForSeconds(0.1f); 
        }
    }

    void CheckDancing()
    {
        if (Input.GetAxisRaw("Submit") != 0)
        {
            if (axisInUse == false)
            {
                //only fire this once every time player presses down on the axis
                if (!axisInUse && myDialogueTrigger.doneWithOneDialogue)
                {
                    if (danceMode)
                    {
                        if (isDancing)
                        {
                            isDancing = false;
                        }
                        else if (!isDancing)
                        {
                            isDancing = true;

                            //only play if it hasn't been played or isn't currently playing
                            //don't want to restart audio
                            if (myAudioManager.themeName != myMusic.thisLevelTheme.name)
                            {
                                myMusic.PlayerInteractAudio();
                            }

                        }
                    }

                    axisInUse = true; 
                }
            }
        }
        if (Input.GetAxisRaw("Submit") == 0)
        {
            axisInUse = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
            isDancing = false; 
        }
    }

}
