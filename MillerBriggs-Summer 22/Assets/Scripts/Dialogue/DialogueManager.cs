using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems; 

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    private Animator layoutAnimator; 
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private Animator panelParentAnimator;
    [SerializeField] private GameObject dialoguePointer; 

    [SerializeField] private float emotion = 0;
    private float growAnimWaitTime = 0.5f; 
    [SerializeField] private float disableUITime = 0.5f; 

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Other Settings")]
    public bool speakerHasName = false;
    public GameObject speakerBox;
    public VoiceLineManager myVoiceManager;
    public Animator npcAnimator, playerAnimator; 

    public static DialogueManager instance {get; private set; }

    private Story currentStory; //current ink file to display

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string EMOTION_TAG = "emotion";
    private const string VOICE_TAG = "vc";
    private const string FOLDER_TAG = "folder";
    private const string ANIMATION_TAG = "anim"; 

    //only allow outside scripts to read the value, not to modify
    public bool dialogueIsPlaying { get; private set; }

    private DialogueVertexAnimator dialogueVertexAnimator;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene"); 
        }
        instance = this;

        dialogueVertexAnimator = new DialogueVertexAnimator(textBox);

        myVoiceManager = FindObjectOfType<VoiceLineManager>().GetComponent<VoiceLineManager>(); 
    }


    private void Start()
    {
        //make sure dialogue is off and UI is disabled
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialoguePointer.SetActive(false); 

        //disable if we don't want speaker to have a name
        
        if (!speakerHasName)
        {
            speakerBox.SetActive(false); 
        }
        else
        {
            //including this for debugging reasons
            speakerBox.SetActive(true); 
        } 

        //Get the layout animator component
        layoutAnimator = dialoguePanel.GetComponent<Animator>(); 

        //get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0; 
        foreach (GameObject obj in choices)
        {
            choicesText[index] = obj.GetComponentInChildren<TextMeshProUGUI>();
            index++; 
        }

    }

    private void Update()
    {
        //Debug.Log(dialogueIsPlaying); 


        //return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }

        //check if dialogue is done, display pointer if so
        if (dialogueIsPlaying)
        {
            //Debug.Log("display pointer"); 
            displayPointer(); 
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //Debug.Log("Enter dialogue mode"); 
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //set portrait, layout, speaker to default values
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("left");

        StartCoroutine(PanelAnimStart());

        ContinueStory();

    }

    IEnumerator PanelAnimStart()
    {
        //panel enlarge
        panelParentAnimator.Play(PanelAnimations.Grow.ToString());

        yield return new WaitForSeconds(growAnimWaitTime);

        panelParentAnimator.Play(PanelAnimations.defaultAnim.ToString());
    }


    private IEnumerator ExitDialogueMode()
    {
        //stop audio 
        myVoiceManager.StopSound();

        //panel shrink
        panelParentAnimator.Play(PanelAnimations.Shrink.ToString());

        yield return new WaitForSeconds(disableUITime); 

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = ""; 
    }

    private Coroutine typeRoutine = null;
    private void ContinueStory()
    {
        if (!dialogueVertexAnimator.textAnimating)
        {
            if (currentStory.canContinue)
            {
                //set text for current dialogue line
                //dialogueText.text = currentStory.Continue();

                //Clear dialogue so you can't see it when swapping views
                dialogueText.text = "";

                this.EnsureCoroutineStopped(ref typeRoutine);
                dialogueVertexAnimator.textAnimating = false;
                List<DialogueCommand> commands = DialogueUtility.ProcessInputString(currentStory.Continue(), out string totalTextMessage);
                typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, null));

                //display any choices for this dialogue
                DisplayChoices();

                HandleTags(currentStory.currentTags);


            }
            else //empty ink JSON file passed in
            {
                StartCoroutine(ExitDialogueMode());
            }
        }
        else
        {
            dialogueVertexAnimator.FinishCurrentAnimation();
            dialogueVertexAnimator.SkipToEndOfCurrentMessage();
        }

    }

    private void HandleTags(List<string> currentTags)
    {
        //Loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            //parse the tag into array of length 2 with key and value
            string[] splitTag = tag.Split(':'); 
            
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag); 
            }

            //split the key and value, trim any whitespace
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle the tag
            switch (tagKey)
            {
                case LAYOUT_TAG:
                    //Debug.Log("layout=" + tagValue);
                    //play animation based on name
                    //dialogueText.text = "";
                    layoutAnimator.Play(tagValue);
                    break;
                case SPEAKER_TAG:
                    //Debug.Log("speaker=" + tagValue);
                    //set text for name
                    displayNameText.text = tagValue; 
                    break;
                case PORTRAIT_TAG:
                    //Debug.Log("portrait=" + tagValue);
                    //play animation based on name
                    portraitAnimator.Play(tagValue); 
                    break;
                case EMOTION_TAG:
                    //Debug.Log("emotion=" + tagValue);
                    //set value for which emotion will be played, using blend trees to swap between values
                    emotion = float.Parse(tagValue);
                    portraitAnimator.SetFloat("emotion", emotion); 
                    break;
                case FOLDER_TAG:
                    //set which folder the voice line is in
                    myVoiceManager.SetFolderLocation(tagValue);
                    break; 
                case VOICE_TAG:
                    //stop the current line in case player skipped text line
                    myVoiceManager.StopSound();
                    myVoiceManager.Play(tagValue);
                    break;
                case ANIMATION_TAG:
                    //SPLIT ANIMATION INTO TWO BITS, ANIMATOR AND TRIGGER NAME
                    string[] splitString = tagValue.Split('_');
                    string animatorName = splitString[0];
                    string triggerName = splitString[1]; 

                    if (npcAnimator != null)
                    {
                        if (npcAnimator.name == animatorName)
                        {
                            npcAnimator.SetTrigger(triggerName);
                            break;
                        }
                    }
                    
                    if (playerAnimator != null)
                    {
                        if (playerAnimator.name == animatorName)
                        {
                            playerAnimator.SetTrigger(triggerName);
                            break; 
                        }
                    }

                    else if (npcAnimator == null && playerAnimator == null)
                    {
                        Debug.LogWarning("Both animators are null!"); 
                    }

                    break; 
                default:
                    Debug.LogWarning(tagKey + " Tag came in but is not currently being handled: " + tag);
                    break; 
            }
        }
    }


    private void DisplayChoices()
    {
        //Choice is an Inky variable type
        List<Choice> currentChoices = currentStory.currentChoices; 

        //defensive check if UI can support num of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Num of choices given: " 
                + currentChoices.Count);
        }

        int index = 0; 
        //Loop through choice objects in the UI and display them according to number for the story
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++; 
        }
        //make sure remaining choices the UI supports are hidden
        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false); 
        }

        StartCoroutine(SelectFirstChoice()); 

    }

    //selects a choice for default so the player can select choices at all
    private IEnumerator SelectFirstChoice()
    {
        //Event System requires we clear it first, then wait for a frame before we set the current selected object
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject); 
    }

    //allows us to register that we have made a choice in inky
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex); 
    }

    void displayPointer()
    {
        bool isTextAnimating = dialogueVertexAnimator.isTextAnimating(); 

        if (isTextAnimating)
        {
            dialoguePointer.SetActive(false); 
        }
        else
        {
            dialoguePointer.SetActive(true); 
        }

    }
    enum PanelAnimations 
    {
        defaultAnim, 
        Grow, 
        Shrink
    }


}
