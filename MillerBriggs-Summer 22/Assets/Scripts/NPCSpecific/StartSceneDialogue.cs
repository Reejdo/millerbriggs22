using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartSceneDialogue : MonoBehaviour
{
    public UnityEvent myEvent;
    public SpriteRenderer mySR;
    public DialogueTrigger myDialogueTrigger;
    public DialogueManager myDialogueManager;
    public PlayerControl myPlayerControl; 

    public Animator playerAnim;
    public string playerLayingAnim; 

    private float fade;
    private float fadeTime = 1.5f;
    private float fadeSpeed = 0.0f;
    private bool startFade;
    private bool triggeredIdle; 



    // Start is called before the first frame update
    void Start()
    {
        myDialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        myPlayerControl = GameObject.Find("player").GetComponent<PlayerControl>();

        startFade = true;
        mySR.gameObject.SetActive(true);
        playerAnim.Play(playerLayingAnim);
        myPlayerControl.SetMoveState(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (startFade)
        {
            if (mySR.color.a != 0)
            {
                Debug.Log("Entry Fade in!");
                fade = Mathf.SmoothDamp(mySR.color.a, 0, ref fadeSpeed, fadeTime);
                mySR.color = new Color(1f, 1f, 1f, fade);
            }
        }

        if (mySR.color.a <= 0.1 && startFade)
        {
            Debug.Log("Start dialogue"); 
            startFade = false;
            TriggerEvent(); 
        }

        if (!triggeredIdle && myDialogueTrigger.hasTalked && !myDialogueManager.dialogueIsPlaying)
        {
            triggeredIdle = false;
            mySR.gameObject.SetActive(false); 
            playerAnim.SetTrigger("idle");
            myPlayerControl.SetMoveState(true);
        }


    }

    void TriggerEvent()
    {
        myEvent.Invoke();
    }
}
