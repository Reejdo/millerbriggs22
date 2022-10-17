using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class No24Animation : MonoBehaviour
{
    public SpriteRenderer mySR;
    public DialogueTrigger myDialogueTrigger;
    public DialogueManager myDialogueManager; 
    public float videoAlpha;
    private float fade;
    [SerializeField] private float fadeTime = 1f;
    private float fadeSpeed = 0.0f;
    public bool fadeIn;

    private int activatedFade = 0; 

    // Start is called before the first frame update
    void Start()
    {
        mySR.gameObject.SetActive(false);
        myDialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        FadeImageSettings();

        if (fadeIn)
        {
            if (mySR.color.a != videoAlpha)
            {
                Debug.Log("Fade in!");
                fade = Mathf.SmoothDamp(mySR.color.a, videoAlpha, ref fadeSpeed, fadeTime);
                mySR.color = new Color(1f, 1f, 1f, fade);
            }
        }

        if (!fadeIn)
        {
            if (mySR.color.a != 0f)
            {
                //Debug.Log("Fade out!"); 
                fade = Mathf.SmoothDamp(mySR.color.a, 0f, ref fadeSpeed, fadeTime);
                mySR.color = new Color(1f, 1f, 1f, fade);
            }
        }

    }


    void FadeImageSettings()
    {
        if (activatedFade == 0)
        {
            if (myDialogueTrigger.hasTalked && myDialogueManager.dialogueIsPlaying)
            {
                fadeIn = true;
                mySR.gameObject.SetActive(true);
            }
            else if (myDialogueTrigger.hasTalked && !myDialogueManager.dialogueIsPlaying)
            {
                fadeIn = false;
                activatedFade = 1; 
            }
        }

        if (mySR.color.a == 0 && !fadeIn && myDialogueTrigger.currentDialogue >= 2)
        {
            mySR.gameObject.SetActive(false);
        }

    }

}
