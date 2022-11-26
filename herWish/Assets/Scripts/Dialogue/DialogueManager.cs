using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //public TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    public Queue<string> sentences;
    //public Queue<float> sentenceTimes;
    public Queue<AudioClip> audioClips; 
    //private Queue<Sprite> portraits;
    [SerializeField] private List<GameObject> UIElements;
    public bool talking;
    public Animator dialogueAnimator;
    public AnimationClip fadeIn, fadeOut, endFadeOut; 

    public float lineCompleteWaitTime;
    public float endDialogueWaitTime;

    private VoiceLineManager myVoiceManager;
    private bool dialogueActive = true; 

    void Start()
    {
        //lvlManage = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        talking = false;
        sentences = new Queue<string>();
        audioClips = new Queue<AudioClip>();

        myVoiceManager = GameObject.FindGameObjectWithTag("VoiceLineManager").GetComponent<VoiceLineManager>(); 
    }

    private void Update()
    {
        CheckDialogueActiveState(); 
    }

    public void StartDialogue(Dialogue dialogue, List<GameObject> newUIElements, TMP_Text newDialogueText)
    {
        if (dialogueActive)
        {
            //Debug.Log("Starting conversation");
            talking = true;

            if (newUIElements.Count > 0)
            {
                for (int i = 0; i < newUIElements.Count; i++)
                {
                    UIElements.Add(newUIElements[i]);
                }

                foreach (GameObject obj in UIElements)
                {
                    obj.SetActive(true);
                }
            }

            dialogueText = newDialogueText;

            if (sentences.Count > 1)
            {
                sentences.Clear();
            }

            //these queues each sentence and each sentence time
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            foreach (AudioClip clip in dialogue.audioClips)
            {
                audioClips.Enqueue(clip);
            }

            StartCoroutine(DisplayNextSentence());
        }

        else
        {
            Debug.Log("Dialogue is not active!"); 
        }
    }

    public IEnumerator DisplayNextSentence()
    {
        //if there are no more sentences
        if (sentences.Count == 0)
        {
            //Play fade out animation
            dialogueAnimator.Play(endFadeOut.name);

            yield return new WaitForSeconds(endDialogueWaitTime); 
            EndDialogue();
  
        }

        while (sentences.Count > 0)
        {
            //Debug.Log("Sentences.Count = " + sentences.Count);

            //Play fade in animation
            dialogueAnimator.Play(fadeIn.name);

            string sentence = sentences.Dequeue();
            AudioClip clip = audioClips.Dequeue();
            float time = clip.length;

            dialogueText.text = sentence;
            myVoiceManager.Play(clip);

            yield return new WaitForSeconds(time);


            if (sentences.Count == 0)
            {
                //Play fade out animation
                dialogueAnimator.Play(endFadeOut.name);
                yield return new WaitForSeconds(lineCompleteWaitTime);
                EndDialogue(); 
            }
            else
            {
                //Play fade out animation
                dialogueAnimator.Play(fadeOut.name);
                yield return new WaitForSeconds(lineCompleteWaitTime);
            }


        }

    }

    public void EndDialogue()
    {
        Debug.Log("End Dialogue"); 

        if (UIElements.Count > 0) 
        {
            foreach (GameObject obj in UIElements)
            {
                obj.SetActive(false);
            }

            UIElements.Clear();
        }

        CheckEndDialogueClear(); 

        talking = false;

    }

    void CheckEndDialogueClear()
    {
        if (sentences.Count > 0)
        {
            sentences.Clear(); 
        }
        if (audioClips.Count > 0)
        {
            audioClips.Clear(); 
        }
    }

    public void QueueAnotherDialogue(Dialogue dialogue)
    {
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (AudioClip clip in dialogue.audioClips)
        {
            audioClips.Enqueue(clip);
        }
    }

    public void SetDialogueActiveState(bool state)
    {
        dialogueActive = state; 
    }

    void CheckDialogueActiveState()
    {
        if (!dialogueActive)
        {
            dialogueText.text = ""; 

            if (talking)
            {
                myVoiceManager.StopSound(); 
                EndDialogue();
            }
        }
    }

}
