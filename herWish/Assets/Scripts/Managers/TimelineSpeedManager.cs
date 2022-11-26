using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class TimelineSpeedManager : MonoBehaviour
{
    public float defaultSpeed = 1f; 
    public float newSpeed;
    private float currentSpeed = 1f; 
    public PlayableDirector myPlayableDirector;

    public float skipTime;
    private float skipTimer, skipTextAlpha = 0f;
    private bool roomFade;
    private float fade;
    private float fadeTime = 1f;
    private float fadeSpeed = 0.0f;

    public TMP_Text skipText, speedUpText;
    public Image roomFader; 
    public UnityEvent LoadScene; 


    public void Start()
    {
        skipText.color = new Color(1f, 1f, 1f, 0f);
        speedUpText.color = new Color(1f, 1f, 1f, 0f);
    }

    public void Update()
    {
        if (skipTimer < skipTime) 
        {
            skipTimer += Time.deltaTime;
        }

        if (skipTimer >= skipTime && skipTextAlpha < 1f)
        {
            skipTextAlpha += Time.deltaTime;
            skipText.color = new Color(1f, 1f, 1f, skipTextAlpha);
            speedUpText.color = new Color(1f, 1f, 1f, skipTextAlpha);
        }

        if (skipTextAlpha >= 1f)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                SkipCutScene();
            }
        }

        //if timeline isn't playing
        if (myPlayableDirector.state != PlayState.Playing)
        {
            SkipCutScene(); 
        }

        if (roomFade)
        {
            if (roomFader.color.a != 1f)
            {
                fade = Mathf.SmoothDamp(roomFader.color.a, 1f, ref fadeSpeed, fadeTime);
                roomFader.color = new Color(0f, 0f, 0f, fade);
            }
        }
    }



    public void SpeedUpCutscene()
    {
        if (skipTimer >= skipTime)
        {
            if (currentSpeed == defaultSpeed)
            {
                Debug.Log("Speed Up!");
                myPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(newSpeed);
                currentSpeed = newSpeed; 
            }
            else if (currentSpeed == newSpeed)
            {
                Debug.Log("Slow Down!");
                myPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(defaultSpeed);
                currentSpeed = defaultSpeed; 
            }
        }
    }

    public void SkipCutScene()
    {
        if (skipTextAlpha >= 1f)
        {
            StartCoroutine(Skip());
        }
    }

    IEnumerator Skip()
    {
        float faderAlpha = roomFader.color.a;

        roomFade = true;

        yield return new WaitForSeconds(fadeTime); 

        LoadScene.Invoke();
    }
}
