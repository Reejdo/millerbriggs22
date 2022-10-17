using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAfterDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueTrigger myDialogueTrigger;
    public DialogueManager myDialogueManager;
    private bool activate;
    public UnityEvent myEvent;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (myDialogueTrigger.hasTalked && !myDialogueManager.dialogueIsPlaying && !activate)
        {
            activate = true;
            TriggerEvent(); 
        }
    }

    void TriggerEvent()
    {
        myEvent.Invoke();
    }
}
