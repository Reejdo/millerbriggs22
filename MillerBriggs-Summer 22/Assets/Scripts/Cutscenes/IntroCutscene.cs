using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public DialogueManager myDialogueManager; 
    public DialogueTrigger dialogueOne, dialogueTwo;
    public GameObject playerDialogueObject, npcObject, npcDialogueObject;
    public SpriteRenderer whiteScreen, playerDialogueIcon; 


    [SerializeField] private float npcWalkTime;
    [SerializeField] private Animator npcAnim;
    private TriggerLoadScene myTriggerScene;

    private bool finishedNPCDialogue = false; 

    // Start is called before the first frame update
    void Start()
    {
        whiteScreen.gameObject.SetActive(true); 
        whiteScreen.color = new Color(1, 1, 1, 0);

        npcObject.SetActive(false);
        playerDialogueObject.SetActive(false);
        npcDialogueObject.SetActive(false);

        myTriggerScene = GetComponent<TriggerLoadScene>(); 

        StartCoroutine(FadeOut()); 

    }

    // Update is called once per frame
    void Update()
    {
        //once the player has finished talking to the npc, load next level
        if (!finishedNPCDialogue && dialogueTwo.currentDialogue > 1 && !myDialogueManager.dialogueIsPlaying)
        {
            finishedNPCDialogue = true;
            StartCoroutine(FadeInAndLoad()); 
        }
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, i);
            yield return new WaitForSeconds(Time.deltaTime); 
        }

        StartCoroutine(BeginCutscene());
    }


    IEnumerator BeginCutscene()
    {
        Debug.Log("Begin"); 


        playerDialogueObject.SetActive(true);

        while (myDialogueManager.dialogueIsPlaying || dialogueOne.currentDialogue < 2)
        {
            yield return null; 
        }

        //wait for dialogue box to disappear
        yield return new WaitForSeconds(0.5f);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            playerDialogueIcon.color = new Color(playerDialogueIcon.color.r, playerDialogueIcon.color.g, playerDialogueIcon.color.b, i);
            yield return null;
        }

        playerDialogueObject.SetActive(false);
        StartCoroutine(NPCWalk()); 

    }

    IEnumerator NPCWalk()
    {
        Debug.Log("Walk");

        npcObject.SetActive(true);
        yield return new WaitForSeconds(npcWalkTime);
        npcAnim.SetBool("stopWalk", true);
        npcDialogueObject.SetActive(true); 
    }

    IEnumerator FadeInAndLoad()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        myTriggerScene.LoadNextScene();
    }


}
