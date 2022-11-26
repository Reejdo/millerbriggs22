using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReactDialogues : MonoBehaviour
{
    private DialogueManager myDialogueManager;
    public DialogueObject[] myDialogueObjects;
    public List<GameObject> UIElements;
    private TMP_Text dialogueText;

    private void Start()
    {
        dialogueText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<TMP_Text>();
        myDialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();

    }

    public void TriggerDialogue(int dialogueNumber)
    {
        if (!myDialogueManager.talking)
        {
            myDialogueManager.StartDialogue(myDialogueObjects[dialogueNumber].dialogue, UIElements, dialogueText);
        }
        else
        {
            myDialogueManager.QueueAnotherDialogue(myDialogueObjects[dialogueNumber].dialogue);
        }
    }

}
