using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    private bool playerInRange;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    [SerializeField] private int maxDialogues; 
    private int currentDialogue = 1; 


    private void Awake()
    {
        playerInRange = false; 
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            //visualCue.SetActive(true); 
            
            if (InputManager.GetInstance().GetInteractPressed())
            {
                //Debug.Log(inkJSON.text); 
                DialogueManager.instance.EnterDialogueMode(inkJSON[currentDialogue - 1]);
                IncreaseDialogueCount();
            }

        }
    }

    void IncreaseDialogueCount()
    {
        if (maxDialogues > 1)
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
