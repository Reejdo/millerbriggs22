using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    private bool playerInRange;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON; 


    private void Awake()
    {
        visualCue.SetActive(false);
        playerInRange = false; 
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            visualCue.SetActive(true); 
            if (InputManager.GetInstance().GetInteractPressed())
            {
                //Debug.Log(inkJSON.text); 
                DialogueManager.instance.EnterDialogueMode(inkJSON);
            }

        }
        else
        {
            visualCue.SetActive(false); 
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
