using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTimeManager : MonoBehaviour
{
    private PlayerControl myPlayerControl;
    private AudioManager myAudioManager; 

    [Header("Time Variables")]
    [SerializeField] private float p_AirTime;
    [SerializeField] private float p_LastAirTime; 
    [SerializeField] private float t_hitGround, t_ExtremeFall, t_FarthestFall;

    private float lastVoiceLineNumber; 

    [Header("Sound Variables")]
    public Sounds hitGround;
    public ReactDialogues voiceLineTriggers;
    public float vcPercentChance;

    //make sure to only queue one dialogue at a time
    private int hitQueueCount = 0; 

    // Start is called before the first frame update
    void Start()
    {
        myPlayerControl = GameObject.FindObjectOfType<PlayerControl>().GetComponent<PlayerControl>();
        myAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myPlayerControl.GetIsGrounded())
        {
            if (myPlayerControl.myRigidBody2D.velocity.y < 0)
            {
                p_AirTime += Time.deltaTime;
                p_LastAirTime = p_AirTime; 
            }
        }
        else if (myPlayerControl.GetIsGrounded())
        {
            if (p_AirTime > 0)
            {
                CheckGroundedInteractions();
                p_AirTime = 0;
                myPlayerControl.myPlayerEffects.SetJumpParticleState(false);
            }
            p_AirTime = 0;
        } 
    }


    void CheckGroundedInteractions()
    {
        //nothing is below hit ground, just return
        if (p_LastAirTime < t_hitGround)
        {
            return; 
        }

        //execute anything below extreme fall
        if (p_LastAirTime < t_ExtremeFall)
        {
            myPlayerControl.myPlayerEffects.PlayGroundParticles();
            myAudioManager.Play(hitGround, true);
            return; 
        }

        //execute anything above extreme fall
        if (p_LastAirTime >= t_ExtremeFall && p_LastAirTime < t_FarthestFall)
        {
            myAudioManager.Play(hitGround, true);
            
            //potentially play voice line for falling
            RandomVoiceLineChance(); 
            return;
        }

        if (p_LastAirTime >= t_FarthestFall)
        {
            myAudioManager.Play(hitGround, true);

            //always play voice line for falling 
            if (hitQueueCount == 0)
            {
                hitQueueCount++;
                Debug.Log("Longest fall, playing vc"); 
                QueueRandomVoiceLine(); 
            }
        }
    }

    void RandomVoiceLineChance()
    {
        float chance = Random.Range(0.0f, 1.0f); 

        //makes it so the voice lines don't happen every time, keeps it more interesting
        if (chance <= vcPercentChance)
        {
            if (hitQueueCount == 0) 
            {
                hitQueueCount++;
                Debug.Log("Chance hit - playing voice line.");
                QueueRandomVoiceLine();
            }

        }
    }


    void QueueRandomVoiceLine()
    {
        int maxRange = voiceLineTriggers.myDialogueObjects.Length; 
        int voiceLine = Random.Range(0, maxRange);

        while (voiceLine == lastVoiceLineNumber)
        {
            voiceLine = Random.Range(0, maxRange);
        }

        lastVoiceLineNumber = voiceLine; 

        //Debug.Log("Playing vc: " + voiceLine); 

        voiceLineTriggers.TriggerDialogue(voiceLine);

        hitQueueCount = 0; 
    }
}
