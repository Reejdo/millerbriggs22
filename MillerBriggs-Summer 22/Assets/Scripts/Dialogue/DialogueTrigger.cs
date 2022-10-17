using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [SerializeField] private bool playerInRange;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    [SerializeField] private int maxDialogues; 
    public int currentDialogue = 1;

    public bool oneTempDialogue;
    public bool oneDialogueAndDone;
    public bool doneWithOneDialogue; 
    public bool hasTalked = false;
    public bool hasFinalDialogue = false; 

    private PlayerControl myPlayerControl;

    [Header("Animatiors")]
    public Animator npcAnimator;
    public Animator playerAnimator; 

    private void Awake()
    {
        playerInRange = false;

        hasTalked = false;

        myPlayerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>(); 

        if (myPlayerControl == null)
        {
            Debug.Log("PlayerControl null!"); 
        }
    }

    private void Update()
    {
        if (!doneWithOneDialogue)
        {
            if (playerInRange && !DialogueManager.instance.dialogueIsPlaying)
            {
                //visualCue.SetActive(true); 

                if (!oneTempDialogue)
                {
                    //Debug.Log("Not one temp"); 

                    if (InputManager.GetInstance().GetInteractPressed())
                    {
                        EnterDialogue();        
                    }
                }
                else if (oneTempDialogue && currentDialogue < 2)
                {
                    //Debug.Log("One temp");

                    if (InputManager.GetInstance().GetInteractPressed())
                    {
                        EnterDialogue(); 
                    }
                }
            }
        }

        if (oneDialogueAndDone && hasTalked)
        {
            visualCue.gameObject.SetActive(false);
        }

        if (oneDialogueAndDone && hasTalked && !DialogueManager.instance.dialogueIsPlaying)
        {
            doneWithOneDialogue = true;
            visualCue.gameObject.SetActive(false); 
        }


    }


    void EnterDialogue()
    {
        hasTalked = true;
        Debug.Log(inkJSON);

        if (npcAnimator != null)
        {
            DialogueManager.instance.npcAnimator = npcAnimator;
            DialogueManager.instance.playerAnimator = playerAnimator; 
        }

        DialogueManager.instance.EnterDialogueMode(inkJSON[currentDialogue - 1]);
        IncreaseDialogueCount();
    }

    public void EventStartDialogue()
    {
        EnterDialogue(); 
        myPlayerControl.DialogueMode(); 
    }


    void IncreaseDialogueCount()
    {
        if (maxDialogues > 1)
        {
            if (!hasFinalDialogue)
            {
                //we never want to go back to the first dialogue
                //set it to the second dialogue if we are at max dialogue
                if (currentDialogue == maxDialogues)
                {
                    currentDialogue = 2;
                }
                else
                {
                    currentDialogue++;
                }
            }
            else if (hasFinalDialogue)
            {
                if (currentDialogue == maxDialogues)
                {
                    currentDialogue = maxDialogues;
                }
                else
                {
                    currentDialogue++;
                }
            }
        }
        else
        {
            currentDialogue = 1;
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
        }
    }
}
