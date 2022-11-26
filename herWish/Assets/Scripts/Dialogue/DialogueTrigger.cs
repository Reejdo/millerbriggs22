using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager myDialogueManager;
    public AllDialogueObjects allDialogues;
    public DialogueObject myDialogueObject;
    public int dialogueNumber; 
    public List<GameObject> UIElements;
    private TMP_Text dialogueText;

    private void Start()
    {
        dialogueText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<TMP_Text>(); 
        myDialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();

        myDialogueObject.hasActivated = allDialogues.hasActivated[dialogueNumber]; 

    }

    public void TriggerDialogue()
    {
        if (!myDialogueObject.onlyActivateOnce || !myDialogueObject.hasActivated)
        {
            if (!myDialogueManager.talking)
            {
                myDialogueManager.StartDialogue(myDialogueObject.dialogue, UIElements, dialogueText);
                allDialogues.SetHasActivated(dialogueNumber, true); 
            }
            else
            {
                myDialogueManager.QueueAnotherDialogue(myDialogueObject.dialogue);
                allDialogues.SetHasActivated(dialogueNumber, true);
            }
        }
        else if (myDialogueObject.onlyActivateOnce && !myDialogueObject.hasActivated)
        {
            if (!myDialogueManager.talking)
            {
                myDialogueManager.StartDialogue(myDialogueObject.dialogue, UIElements, dialogueText);
                allDialogues.SetHasActivated(dialogueNumber, true);
            }
            else
            {
                myDialogueManager.QueueAnotherDialogue(myDialogueObject.dialogue);
                allDialogues.SetHasActivated(dialogueNumber, true);
            }
        }
    }

}