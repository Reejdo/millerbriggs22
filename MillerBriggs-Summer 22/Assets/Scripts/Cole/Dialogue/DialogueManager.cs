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
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator; 

    [SerializeField]
    private float emotion = 0;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText; 


    public static DialogueManager instance {get; private set; }

    private Story currentStory; //current ink file to display

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string EMOTION_TAG = "emotion";




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
    }

    /*
    public static DialogueManager GetInstance()
    {
        return instance; 
    }
    */ 


    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

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
        //return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }

    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory(); 
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f); 

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = ""; 
    }

    private Coroutine typeRoutine = null;
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //set text for current dialogue line
            //dialogueText.text = currentStory.Continue();


            this.EnsureCoroutineStopped(ref typeRoutine);
            dialogueVertexAnimator.textAnimating = false;
            List<DialogueCommand> commands = DialogueUtility.ProcessInputString(currentStory.Continue(), out string totalTextMessage);
            typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, null));

            //display any choices for this dialogue
            DisplayChoices();

            //handle tags
            HandleTags(currentStory.currentTags); 

        }
        else //empty ink JSON file passed in
        {
            StartCoroutine(ExitDialogueMode());
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
                case SPEAKER_TAG:
                    //Debug.Log("speaker=" + tagValue);
                    displayNameText.text = tagValue; 
                    break;
                case PORTRAIT_TAG:
                    //Debug.Log("portrait=" + tagValue);
                    //play animation based on name
                    portraitAnimator.Play(tagValue); 
                    break;
                case LAYOUT_TAG:
                    //Debug.Log("layout=" + tagValue);
                    break;
                case EMOTION_TAG:
                    //Debug.Log("emotion=" + tagValue);
                    emotion = float.Parse(tagValue);
                    portraitAnimator.SetFloat("emotion", emotion); 
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
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

}
